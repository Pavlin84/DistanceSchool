﻿namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
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

        [Display(Name = "Профилна снимка")]
        public IFormFile ProfileImage { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [Display(Name = "Автобиография")]
        public IFormFile ApplicationDocuments { get; set; }

    }
}
