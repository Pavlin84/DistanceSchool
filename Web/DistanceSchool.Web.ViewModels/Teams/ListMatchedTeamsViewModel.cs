namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    public class ListMatchedTeamsViewModel
    {
        public ListMatchedTeamsViewModel()
        {
            this.MatchedTeams = new HashSet<MatchedTeamsViewModel>();
        }
        public ICollection<MatchedTeamsViewModel> MatchedTeams { get; set; }
    }
}
