
namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Data.Models;

    public class AddTeamInputModel : TeamBaseViewModel
    {

        public int SchoolId { get; set; }

        [Required]
        public TeamLevel TeamLevel { get; set; }

        public ICollection<int> DisciplinesId { get; set; }
    }
}
