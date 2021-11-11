namespace DistanceSchool.Web.ViewModels.Administration.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DistanceSchool.Web.ViewModels.Candidacyes;
    using Microsoft.AspNetCore.Http;

    public class BaseHomePageViewModel
    {
        public BaseHomePageViewModel()
        {
            this.Candidacies = new HashSet<CandidacyViewModel>();
        }

        public ICollection<CandidacyViewModel> Candidacies { get; set; }
    }
}
