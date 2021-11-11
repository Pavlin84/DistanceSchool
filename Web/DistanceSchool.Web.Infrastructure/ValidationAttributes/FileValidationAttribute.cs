namespace DistanceSchool.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    using Microsoft.AspNetCore.Http;

    public class FileValidationAttribute : ValidationAttribute
    {
        private readonly string[] validExtentions;
        private readonly int fileSize;

        public FileValidationAttribute(params string[] extentions)
        {
            this.validExtentions = extentions;
        }

        public FileValidationAttribute(int fileSize, params string[] extentions)
              : this(extentions)
        {
            this.fileSize = fileSize;
        }



        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var fileEtention = Path.GetExtension(file.FileName).TrimStart('.');

                if (this.fileSize != 0 && file.Length > this.fileSize * 1024 * 1024)
                {
                    return new ValidationResult($"Максималния допустим размер е {this.fileSize} Mb");
                }

                foreach (var validExtention in this.validExtentions)
                {
                    if (fileEtention.ToLower() == validExtention.ToLower())
                    {
                        return ValidationResult.Success;
                    }
                }

                return new ValidationResult("Допускат се следните файлови формат: ." + string.Join(", .", this.validExtentions));
            }

            //throw new ArgumentException("Invalid File (File is not IFormFile)");
            return ValidationResult.Success;
        }
    }
}
