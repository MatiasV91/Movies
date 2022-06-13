using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Helper
{
    public class ValidImage : ValidationAttribute
    {
        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IFormFile)value;
            if(file != null)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!permittedExtensions.Contains(ext))
                {
                    return new ValidationResult("Only supported Image formats are JPG and PNG");
                }
            }
            return ValidationResult.Success;
        }
    }
}
