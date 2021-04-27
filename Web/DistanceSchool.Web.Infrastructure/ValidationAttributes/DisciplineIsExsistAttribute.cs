using DistanceSchool.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace DistanceSchool.Web.Infrastructure.ValidationAttributes
{
    public class DisciplineIsExsistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (IDisciplineAttributeService)validationContext.GetService(typeof(IDisciplineAttributeService));
            var discipline = (string)value;
            if (service.IsDisciplineExsist(discipline))
            {
                return new ValidationResult("Дисциплината вече съществува");
            }

            return ValidationResult.Success;
        }
    }
}
