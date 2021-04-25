namespace DistanceSchool.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserYearValidationAttribute : ValidationAttribute
    {
        private readonly int minYears;

        public UserYearValidationAttribute(int minYears)
        {
            this.minYears = minYears;
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime birthDate)
            {
                return birthDate.Date.AddYears(this.minYears) <= DateTime.UtcNow.Date;
            }

            return false;
        }
    }
}
