namespace DistanceSchool.Web.ViewModels.Administration.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DistanceSchool.Web.ViewModels.Candidacyes;

    public class AdministrationHomeViewModel : BaseHomePageViewModel
    {
        public AdministrationHomeViewModel()
        {
            this.Disciplines = new HashSet<string>();
        }

        public ICollection<string> Disciplines { get; set; }

    }
}
