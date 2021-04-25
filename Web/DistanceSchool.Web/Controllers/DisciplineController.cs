namespace DistanceSchool.Web.Controllers
{
    using System.Threading.Tasks;

    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using Microsoft.AspNetCore.Mvc;

    public class DisciplineController : BaseController
    {
        private readonly IDisciplineService disciplineService;

        public DisciplineController(IDisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }

        // TODO add constraints
        public async Task<IActionResult> RemoveToSchool(int disciplineId, int schoolId)
        {
            await this.disciplineService.RemoveDisciplineFromSchoolAsync(disciplineId, schoolId);
            return this.RedirectToAction("OneSchool", "School", new { Id = schoolId });
        }

        // TO DO onlu for administrator and school manager
        public async Task<IActionResult> AddToSchool(int disciplineId, int schoolId)
        {
            await this.disciplineService.AddDisciplineToSchoolAsync(disciplineId, schoolId);

            return this.RedirectToAction(
                nameof(SchoolController.OneSchool),
                nameof(SchoolController).ToString().Replace("Controller", string.Empty),
                new { Id = schoolId });
        }

        // TO DO only administrator
        public IActionResult CreateDiscipline()
        {
            return this.View();
        }

        // TO DO only administrator
        [HttpPost]
        public async Task<IActionResult> CreateDiscipline(CreateDisciplineInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.disciplineService.CreateDisciplineAsync(inputModel);

            return this.Redirect("/");
        }
    }
}
