using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Neama.Core.Entities.Identity
{
    public class EmailOrPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = value as string;

            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult("The field is required.");
            }

            // Check if input is a valid email
            if (Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return ValidationResult.Success;
            }

            // Check if input is a valid phone number (basic example, you can customize it)
            if (Regex.IsMatch(input, @"^\+?[0-9]\d{1,14}$")) // E.164 format for phone numbers
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The field must be a valid email address or phone number.");
        }
    }
}
