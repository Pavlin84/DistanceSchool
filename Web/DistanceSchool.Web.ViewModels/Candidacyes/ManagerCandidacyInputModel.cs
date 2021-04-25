namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using DistanceSchool.Common;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;

    public class ManagerCandidacyInputModel : BaseCandidacyModel
    {
        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [DataType(DataType.Date)]
        [UserYearValidation(21, ErrorMessage ="Трябва да имате навършени 21 години")]
        public DateTime BirthDate { get; set; }

        public int SchoolId { get; set; }
    }
}
