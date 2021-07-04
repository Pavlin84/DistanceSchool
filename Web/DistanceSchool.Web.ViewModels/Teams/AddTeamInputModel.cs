namespace DistanceSchool.Web.ViewModels.Teams
{

    using DistanceSchool.Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class AddTeamInputModel : TeamBaseViewModel
    {

        public int SchoolId { get; set; }

        [Required]
        public TeamLevel TeamLevel { get; set; }
    }
}
