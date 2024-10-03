using FurniFusion.Dtos;
using FurniFusion.Dtos.Auth;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Transactions;

namespace FurniFusion.Controllers
{
    [ApiController]
    [Route(("api/[controller]"))]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;


        public AuthController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                try
                {

                    var user = new User
                    {
                        UserName = registerDto.Username,
                        FirstName = registerDto.FirstName!,
                        LastName = registerDto.LastName!,
                        Email = registerDto.Email,
                    };

                    // Create the user
                    var createdUser = await _userManager.CreateAsync(user, registerDto.Password!);

                    if (!createdUser.Succeeded)
                        return BadRequest(createdUser.Errors);

                    // Add the user to the role
                    var roleResult = await _userManager.AddToRoleAsync(user, "user");

                    if (!roleResult.Succeeded)
                        return BadRequest(roleResult.Errors);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = encodedToken }, Request.Scheme);

                    var template = await _emailService.GetEmailTemplate("emailVerification");

                    // Queue the email sending as a background job
                    BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(user.Email!, "Confirm Your FurniFusion Email", template.Replace("{{confirmationLink}}", confirmationLink!)));

                    // Commit the transaction
                    scope.Complete();

                    // Return 201 Created with the new user data
                    return CreatedAtAction(nameof(Register), new { id = user.Id });
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var user = await _userManager.FindByEmailAsync(loginDto.Email!);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Unauthorized(new { message = "Please confirm your email before logging in." });
                }

                // Find user by email
                var result = await _signInManager.PasswordSignInAsync(user.UserName!, loginDto.Password!, isPersistent: false, lockoutOnFailure: true);


                if (result.IsLockedOut)
                    return Unauthorized(new
                    {
                        message = "Your account is locked out due to too many failed attempts."
                    });


                if (!result.Succeeded)
                    return Unauthorized(new
                    {
                        message = "Invalid email or password."
                    });

                // Generate JWT token

                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.CreateToken(user, userRoles);



                // Return the login response with user data and token
                return Ok(new LoginResponseDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = await _userManager.GetRolesAsync(user) as List<string>,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok("User has been logged out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("sendEmailVerification")]
        public async Task<IActionResult> SendEmailVerification([FromBody] SendEmailVerificationDto sendEmailVerification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var user = await _userManager.FindByEmailAsync(sendEmailVerification.Email!);

                if (user == null)
                    return BadRequest("Invalid request!");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var template = await _emailService.GetEmailTemplate("emailVerification");

                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = encodedToken }, Request.Scheme);

                // Queue the email sending as a background job
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(user.Email!, "Confirm Your FurniFusion Email", template.Replace("{{confirmationLink}}", confirmationLink!)));

                return Ok("Email verification link has been sent to your email!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Decode the Base64 URL-encoded token
            byte[] decodedBytes = WebEncoders.Base64UrlDecode(token);

            // Convert the byte array back to a string
            string decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpPost("sendResetPasswordEmail")]
        public async Task<IActionResult> SendResetPassword([FromBody] SendResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email!);

                if (user == null)
                {
                    return BadRequest("Invalid Request!");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var resetLink = Url.Action("ResetPassword", "Auth", new { userEmail = user.Email, token = encodedToken }, Request.Scheme);

                var template = await _emailService.GetEmailTemplate("forgotPassword");
                // Queue the email sending as a background job
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(user.Email!, "Reset Password", template.Replace("{{resetLink}}", resetLink!)));


                return Ok("Reset password link has been sent to your email");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email!);

                if (user == null)
                {
                    return BadRequest("Invalid Request!");
                }

                var decodedToken = WebUtility.UrlDecode(resetPassword.Token);

                var result = await _userManager.ResetPasswordAsync(user, decodedToken!, resetPassword.NewPassword!);

                if (result.Succeeded)
                {
                    return Ok("Password has been reset successfully");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { Errors = errors });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
