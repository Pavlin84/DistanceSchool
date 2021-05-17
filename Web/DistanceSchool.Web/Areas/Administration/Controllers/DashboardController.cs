namespace DistanceSchool.Web.Areas.Administration.Controllers
{
    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Controllers;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IDisciplineService disciplineService;
        private readonly ICandidacyServices candidacyService;

        public DashboardController(ISettingsService settingsService, IDisciplineService disciplineService, ICandidacyServices candidacyService)
        {
            this.settingsService = settingsService;
            this.disciplineService = disciplineService;
            this.candidacyService = candidacyService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult AdminHome()
        {
            var viewModel = new AdministrationHomeViewModel();
            viewModel.Disciplines = this.disciplineService.GetAllDiscpline();
            viewModel.Candidacyes = this.candidacyService.GetAllManagerCandidacy();
            this.ViewData["CreateActionName"] = nameof(SchoolController.AddManager);
            return this.View(viewModel);
        }
    }
}
