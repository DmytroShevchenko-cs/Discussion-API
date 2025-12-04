namespace Discussion.Core.Infrastructure.Common.Attribute;

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class FileValidatorAttribute(
    string extensions,
    int maxSizeBytes = 0,
    bool isOptional = false) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (isOptional && value == null)
        {
            return ValidationResult.Success;
        }

        var allowedExtensions = extensions.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim().StartsWith('.')
                ? e.Trim().ToLowerInvariant()
                : "." + e.Trim().ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (value is IFormFile singleFile)
        {
            return ValidateFile(singleFile, allowedExtensions);
        }

        if (value is not IEnumerable<IFormFile> files)
        {
            return new ValidationResult("Please select a file");
        }

        foreach (var fileItem in files)
        {
            var validationResult = ValidateFile(fileItem, allowedExtensions);
            if (!string.IsNullOrEmpty(validationResult?.ErrorMessage))
            {
                return validationResult;
            }
        }

        return ValidationResult.Success;
    }

    private ValidationResult? ValidateFile(IFormFile fileItem, HashSet<string> allowedExtensions)
    {
        if (fileItem.Length == 0)
        {
            return new ValidationResult("Provide not empty file");
        }

        var extensionItem = Path.GetExtension(fileItem.FileName);
        if (allowedExtensions.Count > 0 && !allowedExtensions.Contains(extensionItem.ToLowerInvariant()))
        {
            return new ValidationResult($"The extension '{extensionItem}' is not allowed! " +
                                        $"Allowed extensions: '{extensions}'");
        }
        
        if (maxSizeBytes > 0 && fileItem.Length > maxSizeBytes)
        {
            return new ValidationResult("Maximum allowed file size is " +
                                        $"{maxSizeBytes / 1024 / 1024} megabytes");
        }

        return ValidationResult.Success;
    }
}