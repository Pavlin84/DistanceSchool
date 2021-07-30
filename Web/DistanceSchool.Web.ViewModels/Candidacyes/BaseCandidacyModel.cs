namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class BaseCandidacyModel
    {
        [BindNever]
        public string UserId { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [MinLength(3, ErrorMessage = GlobalConstants.CyrillicMinLenghtErrorMessage)]
        [MaxLength(30, ErrorMessage = GlobalConstants.CyrillicMaxLenghtErrorMessage)]
        [Display(Name ="Име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [MinLength(3, ErrorMessage = GlobalConstants.CyrillicMinLenghtErrorMessage)]
        [MaxLength(30, ErrorMessage = GlobalConstants.CyrillicMaxLenghtErrorMessage)]
        [Display(Name = "Бащино име")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [MinLength(3, ErrorMessage = GlobalConstants.CyrillicMinLenghtErrorMessage)]
        [MaxLength(30, ErrorMessage = GlobalConstants.CyrillicMaxLenghtErrorMessage)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [BindNever]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [FileValidation(8, "pdf", "doc")]
        public virtual IFormFile ApplicationDocuments { get; set; }
    }
}
