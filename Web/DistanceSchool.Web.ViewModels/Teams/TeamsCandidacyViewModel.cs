namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Candidacyes;

    public class TeamsCandidacyViewModel : TeamBaseViewModel
    {
        public TeamsCandidacyViewModel()
        {
            this.Candidacies = new HashSet<CandidacyViewModel>();
        }

        public ICollection<CandidacyViewModel> Candidacies { get; set; }
    }
}
