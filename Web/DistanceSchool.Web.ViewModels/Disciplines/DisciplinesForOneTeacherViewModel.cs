namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Teams;

    public class DisciplinesForOneTeacherViewModel
    {
        public string Name { get; set; }

        public List<TeamBaseViewModel> Teams { get; set; }

    }
}
