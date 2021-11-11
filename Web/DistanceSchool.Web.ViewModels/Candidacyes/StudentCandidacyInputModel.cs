namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class StudentCandidacyInputModel : CandidacyInputModel
    {
        [UserYearValidation(7, ErrorMessage = "Трябва да имате навършени 7 години")]
        public override DateTime BirthDate { get => base.BirthDate; set => base.BirthDate = value; }
    }
}
