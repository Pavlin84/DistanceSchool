namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class TeamBaseViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [MaxLength(8, ErrorMessage = GlobalConstants.CyrillicMaxLenghtTeamNameErrorMessage)]
        [MinLength(2, ErrorMessage = GlobalConstants.CyrillicMinLenghtTeamNameErrorMessage)]
        public string TeamName { get; set; }
    }
}
