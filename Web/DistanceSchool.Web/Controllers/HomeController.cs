namespace DistanceSchool.Web.Controllers
{
    using System.Diagnostics;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.ViewModels;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IDisciplineService disciplineService;
        private readonly ICandidacyServices candidacyServices;

        public HomeController(IDisciplineService disciplineService, ICandidacyServices candidacyServices)
        {
            this.disciplineService = disciplineService;
            this.candidacyServices = candidacyServices;
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
