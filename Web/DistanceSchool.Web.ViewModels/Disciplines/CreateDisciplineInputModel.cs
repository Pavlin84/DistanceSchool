namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Web.Infrastructure.ValidationAttributes;

    public class CreateDisciplineInputModel
    {
        [Required(ErrorMessage ="Моля въведете име")]

        [MinLength(5, ErrorMessage ="Моля въведете име по дълго от 5 символа")]
        [DisciplineIsExsist]
        [MaxLength(20, ErrorMessage ="Моля Въведете име по кратко от 20 символа")]
        [Display(Name="Име на дисциплината:")]
        public string Name { get; set; }

    }
}
