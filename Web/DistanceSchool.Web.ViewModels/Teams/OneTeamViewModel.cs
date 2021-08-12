namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Students;

    public class OneTeamViewModel : TeamBaseViewModel
    {
        public OneTeamViewModel()
        {
            this.Students = new List<StudentForOneTeamViewModel>();
            this.Disciplines = new List<DisciplineForOneTeamViewModel>();
        }

        public string SchoolName { get; set; }

        public ICollection<StudentForOneTeamViewModel> Students { get; set; }

        public ICollection<DisciplineForOneTeamViewModel> Disciplines { get; set; }

        public bool IsUserManager { get; set; }

        public bool IsTeachesToTeam { get; set; }
    }
}
