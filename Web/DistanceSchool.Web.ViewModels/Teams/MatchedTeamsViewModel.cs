namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    public class MatchedTeamsViewModel
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public string SchoolName { get; set; }

        public ICollection<string> MatchedDisciplines { get; set; }

        public ICollection<string> NotMatchedDisciplines { get; set; }
    }
}
