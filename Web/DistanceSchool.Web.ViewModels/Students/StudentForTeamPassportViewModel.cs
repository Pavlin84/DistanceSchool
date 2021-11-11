namespace DistanceSchool.Web.ViewModels.Students
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DistanceSchool.Web.ViewModels.Exams;
    using DistanceSchool.Web.ViewModels.Lessons;

    public class StudentForTeamPassportViewModel : StudentBaseViewModel
    {
        public ExamViewModel Exam { get; set; }

        public List<LessonViewModel> Lessons { get; set; }
    }
}
