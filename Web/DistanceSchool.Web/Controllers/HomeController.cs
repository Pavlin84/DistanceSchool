namespace DistanceSchool.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using DistanceSchool.Services.Data;
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
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        // TO DO Create constraints
        public IActionResult Administration()
        {
            var viewModel = new AdministrationHomeViewModel();
            viewModel.Disciplines = this.disciplineService.GetAllDiscpline();
            viewModel.Candidacyes = this.candidacyServices.GetAllManagerCandidacy();
            this.ViewData["ActionName"] = nameof(SchoolController.AddManager);
            return this.View(viewModel);
        }

        // TO DO Create constraints
        [Authorize]
        public IActionResult SchoolManager()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewModel = this.schoolService.GetManagerHomePageData(userId);
            return this.View(viewModel);
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
