namespace DistanceSchool.Web.ViewModels.Teachers
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Disciplines;

    public class OneTeachetViewModel
    {
        public string Id { get; set; }

        public string TeacherNames { get; set; }

        public string SchoolName { get; set; }

        public List<DisciplinesForOneTeacherViewModel> Disciplines { get; set; }
    }
}
