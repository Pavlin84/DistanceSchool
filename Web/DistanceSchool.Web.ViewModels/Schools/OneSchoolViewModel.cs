namespace DistanceSchool.Web.ViewModels.Schools
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;

    public class OneSchoolViewModel : SchoolBaseModel
    {
        public OneSchoolViewModel()
        {
            this.Manager = new TeacherForOneSchoolViewModel();
            this.Teacher = new List<TeacherForOneSchoolViewModel>();
            this.Teams = new List<TeamForOneSchoolViewModel>();
            this.Disciplines = new List<DisciplineForOneSchoolViewModel>();
            this.NotStudiedDisciplines = new List<DisciplineForOneSchoolViewModel>();
        }

        public TeacherForOneSchoolViewModel Manager { get; set; }

        public bool IsUserManager { get; set; }

        public ICollection<TeamForOneSchoolViewModel> Teams { get; set; }

        public ICollection<TeacherForOneSchoolViewModel> Teacher { get; set; }

        public ICollection<DisciplineForOneSchoolViewModel> Disciplines { get; set; }

        public ICollection<DisciplineForOneSchoolViewModel> NotStudiedDisciplines { get; set; }

    }
}
