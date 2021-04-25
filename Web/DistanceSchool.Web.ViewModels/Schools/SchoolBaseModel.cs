
namespace DistanceSchool.Web.ViewModels.Schools
{
    using System.ComponentModel.DataAnnotations;

    public abstract class SchoolBaseModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Името е задължително")]
        [MinLength(3,ErrorMessage ="Името рябва да се с повече от 3 символа")]
        [MaxLength(80,ErrorMessage = "Името трябва да е по-малко от 80 символа")]
        [Display(Name ="Име на училището:")]
        public string Name { get; set; }

        [Display(Name = "Описание:")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Адресът е задължителен")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
    }
}
