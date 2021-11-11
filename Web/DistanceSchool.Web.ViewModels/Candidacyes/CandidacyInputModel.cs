namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class CandidacyInputModel : BaseCandidacyModel
    {
        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на раждане")]
        public virtual DateTime BirthDate { get; set; }

        [Display(Name = "Профилна снимка")]
        [FileValidation(7, "jpg", "jpeg", "png", "gif")]
        public IFormFile ProfileImage { get; set; }

        public int SchoolId { get; set; }

        public int TeamId { get; set; }

        [Display(Name = "Докуменнти за кандидатстване")]
        public override IFormFile ApplicationDocuments { get => base.ApplicationDocuments; set => base.ApplicationDocuments = value; }

        public string CandidacyTypeMessage { get; set; }

    }
}
