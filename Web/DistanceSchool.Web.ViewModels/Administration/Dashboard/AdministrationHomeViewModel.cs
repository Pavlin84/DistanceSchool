namespace DistanceSchool.Web.ViewModels.Administration.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DistanceSchool.Web.ViewModels.Candidacyes;

    public class AdministrationHomeViewModel
    {
        public AdministrationHomeViewModel()
        {
            this.Disciplines = new HashSet<string>();
            this.Candidacyes = new HashSet<MangerCandidacyViewModel>();
        }

        public ICollection<string> Disciplines { get; set; }

        public ICollection<MangerCandidacyViewModel> Candidacyes { get; set; }
    }
}
