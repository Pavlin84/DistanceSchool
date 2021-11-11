namespace DistanceSchool.Web.ViewModels.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Services.Mapping;
    using DistanceSchool.Web.ViewModels.Exams;
    using DistanceSchool.Web.ViewModels.Students;

    public class TeamPassportVewModel : TeamBaseViewModel
    {

        public string Discipline { get; set; }

        public List<StudentForTeamPassportViewModel> Students { get; set; }

        public List<string> Lessons { get; set; }

        public ExamViewModel Exam { get; set; }

    }
}
