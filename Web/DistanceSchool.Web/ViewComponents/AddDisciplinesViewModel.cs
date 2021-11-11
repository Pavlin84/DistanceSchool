namespace DistanceSchool.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddDisciplinesViewModel
    {
        [Display(Name = "Дисциплини")]
        public ICollection<SelectListItem> DisciplinesId { get; set; }
    }
}
