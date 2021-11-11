namespace DistanceSchool.Web.ViewModels.Managers
{
    using System.Collections.Generic;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using DistanceSchool.Web.ViewModels.Candidacyes;

    public class SchoolManagerHomeViewModel : BaseHomePageViewModel
    {
        public SchoolManagerHomeViewModel()
        {
        }

        public int SchoolId { get; set; }

        public string SchoolManager { get; set; }

        public string SchoolName { get; set; }
    }
}
