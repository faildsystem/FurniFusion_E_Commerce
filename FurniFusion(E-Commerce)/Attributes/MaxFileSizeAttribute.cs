using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FurniFusion.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get IConfiguration from the service provider
            var configuration = validationContext.GetService(typeof(IConfiguration)) as IConfiguration;

            // Handle null configuration (this should not happen in properly configured applications)
            if (configuration == null)
            {
                return new ValidationResult("Configuration is not available.");
            }

            // Get max file size from the configuration (in MB)
            var maxFileSizeInMB = configuration.GetValue<int>("ImageSettings:MaxFileSizeInMB");
            var maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024; // Convert MB to bytes

            // Validate the file size
            var file = value as IFormFile;
            if (file != null && file.Length > maxFileSizeInBytes)
            {
                var errorMessage = $"Maximum allowed file size is {maxFileSizeInMB} MB.";
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
