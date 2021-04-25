//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//using DistanceSchool.Services.Data;

//namespace DistanceSchool.Web.Infrastructure.ValidationAttributes
//{
//    public class DisciplineIsExsistAttribute : ValidationAttribute
//    {
//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            var service = (IDisciplineService)validationContext.GetService(typeof(IDisciplineService));
//            var discipline = (string)value;
//            if (service.IsExsist(discipline))
//            {
//                return new ValidationResult("Дисциплината вече съществува");
//            }

//            return ValidationResult.Success;
//        }
//    }
//}
