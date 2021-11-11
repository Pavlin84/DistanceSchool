namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class TeacherCandidacyInputModel : CandidacyInputModel
    {
        [UserYearValidation(21, ErrorMessage = "Трябва да имате навършени 21 години")]
        public override DateTime BirthDate { get => base.BirthDate; set => base.BirthDate = value; }


        [BindNever]
        public bool IsAlreadyTeacher { get; set; }
    }
}
