namespace DistanceSchool.Web.ViewModels.Lessons
{
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Common;
    using DistanceSchool.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class CreateAndAddLessonViewModel : AddLessonBaseViewModel
    {
        [Display(Name = "Име на урока")]
        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        [MinLength(5, ErrorMessage = "Името трябва да съдържа поне 5 символа")]
        [MaxLength(30, ErrorMessage = "Името трябва да е с най-много 30 символа")]
        public string NewLessonName { get; set; }

        [Display(Name = "Качи урок :")]
        [FileValidation(200, "pptx", "doc", "xls", "pdf")]
        [Required(ErrorMessage = GlobalConstants.CyrillicRequiredFieldMessage)]
        public IFormFile NewLessonData { get; set; }
    }
}
