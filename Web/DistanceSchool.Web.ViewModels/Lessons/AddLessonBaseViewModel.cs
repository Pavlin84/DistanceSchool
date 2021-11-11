namespace DistanceSchool.Web.ViewModels.Lessons
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using DistanceSchool.Common;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Services.Mapping;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class AddLessonBaseViewModel : IMapFrom<TeacherTeam>
    {

        public int Id { get; set; }

        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public TeamLevel TeamLevel { get; set; }

        public string DisciplineName { get; set; }

        public int DisciplineId { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на провеждане")]
        [LessonStudyDateValidation(ErrorMessage ="Избрали сте твърде ранна дата")]
        public DateTime DateOfStudy { get; set; }
    }
}
