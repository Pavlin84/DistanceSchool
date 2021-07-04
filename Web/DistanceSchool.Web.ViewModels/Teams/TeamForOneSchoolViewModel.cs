namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    public class TeamForOneSchoolViewModel
    {
        public TeamForOneSchoolViewModel()
        {
            this.TecherWhithDisciplines = new List<string>();
        }

        public int Id { get; set; }

        public string TeamName { get; set; }

        public ICollection<string> TecherWhithDisciplines { get; set; }
    }
}
