﻿namespace DistanceSchool.Web.Areas.Administration.Controllers
{
    using System.Security.Claims;

    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Controllers;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IDisciplineService disciplineService;
        private readonly ICandidacyServices candidacyService;
        private readonly ISchoolService schoolService;

        public DashboardController(
            ISettingsService settingsService,
            IDisciplineService disciplineService,
            ICandidacyServices candidacyService,
            ISchoolService schoolService)
        {
            this.settingsService = settingsService;
            this.disciplineService = disciplineService;
            this.candidacyService = candidacyService;
            this.schoolService = schoolService;
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

        [AdminManagerAuthorizeAttribute]
        public IActionResult SchoolManagerHome(int id)
        {
            this.ViewData["CreateActionName"] = nameof(SchoolController.AddTeacher);

            if (id != 0)
            {
                var viewModel = this.schoolService.GetManagerHomePageBySchholId(id);
                return this.View(viewModel);
            }
            else
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var viewModel = this.schoolService.GetManagerHomePageData(userId);
                return this.View(viewModel);
            }

        }
    }
}
