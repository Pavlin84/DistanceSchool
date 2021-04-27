namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ManagerCandidacyInputModel : BaseCandidacyModel
    {
        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [DataType(DataType.Date)]
        [UserYearValidation(21, ErrorMessage ="Трябва да имате навършени 21 години")]
        public DateTime BirthDate { get; set; }

        public int SchoolId { get; set; }

        [BindNever]
        public bool IsAlreadyTeacher { get; set; }
    }
}
