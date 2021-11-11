namespace DistanceSchool.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LessonStudyDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime studyDate)
            {
                return studyDate.Date > DateTime.UtcNow.Date;
            }

            return false;
        }
    }
}
