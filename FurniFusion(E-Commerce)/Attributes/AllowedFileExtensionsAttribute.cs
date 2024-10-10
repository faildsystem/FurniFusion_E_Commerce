using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

public class AllowedFileExtensionsAttribute : ValidationAttribute
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

        // Get allowed extensions from the configuration
        var allowedExtensions = configuration.GetValue<string>("ImageSettings:AllowedExtensions")?.Split(',');

        // Handle null or empty allowed extensions
        if (allowedExtensions == null || !allowedExtensions.Any())
        {
            return new ValidationResult("Allowed file extensions are not configured.");
        }

        // Validate the file extension
        var file = value as IFormFile;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return new ValidationResult("This file extension is not allowed.");
            }
        }

        return ValidationResult.Success;
    }
}
