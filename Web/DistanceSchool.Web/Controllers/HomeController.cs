namespace DistanceSchool.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Areas.Administration.Controllers;
    using DistanceSchool.Web.ViewModels;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using DistanceSchool.Web.ViewModels.Managers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IDisciplineService disciplineService;
        private readonly ICandidacyServices candidacyServices;
        private readonly ISchoolService schoolService;

        public HomeController(
            IDisciplineService disciplineService,
            ICandidacyServices candidacyServices,
            ISchoolService schoolService)
        {
            this.disciplineService = disciplineService;
            this.candidacyServices = candidacyServices;
            this.schoolService = schoolService;
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (this.schoolService.IsUserManger(userId))
            {
                return this.Redirect("/Administration/Dashboard/SchoolManagerHome");
            }

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Redirect("/Administration/Dashboard/AdminHome");
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult Sucsess(string message)
        {
            return this.View((object)message);
        }
    }
}
