namespace DistanceSchool.Web.ViewModels.Teams
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel.DataAnnotations;

    public class TeamBaseViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(2)]
        public string TeamName { get; set; }
    }
}
