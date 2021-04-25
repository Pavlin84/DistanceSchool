namespace DistanceSchool.Web.ViewModels.Teams
{

    using DistanceSchool.Data.Models;

    public class AddTeamInputModel : TeamBaseViewModel
    {

        public string SchoolId { get; set; }

        public TeamLevel TeamLevel { get; set; }
    }
}
