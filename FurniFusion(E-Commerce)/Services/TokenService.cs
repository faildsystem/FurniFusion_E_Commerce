using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurniFusion.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));
        }
        public string CreateToken(User appUser, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName!),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id) // This claim holds the UserId
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var creds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };


            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }
    }
}



