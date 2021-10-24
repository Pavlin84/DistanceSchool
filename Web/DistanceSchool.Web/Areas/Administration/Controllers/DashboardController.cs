namespace DistanceSchool.Web.Areas.Administration.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Controllers;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    public class DashboardController : AdministrationController
    {
        private readonly IDisciplineService disciplineService;
        private readonly ICandidacyServices candidacyService;
        private readonly ISchoolService schoolService;
        private readonly ITeacherService teacherService;
        private readonly IWebHostEnvironment environment;
        private readonly IDistributedCache distributedCache;

        public DashboardController(
            IDisciplineService disciplineService,
            ICandidacyServices candidacyService,
            ISchoolService schoolService,
            ITeacherService teacherService,
            IWebHostEnvironment environment,
            IDistributedCache distributedCache)
        {
            this.disciplineService = disciplineService;
            this.candidacyService = candidacyService;
            this.schoolService = schoolService;
            this.teacherService = teacherService;
            this.environment = environment;
            this.distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> AdminHome()
        {
            AdministrationHomeViewModel viewModel;

            var stringCacheInfo = await this.distributedCache.GetStringAsync("MemoryData");

            if (stringCacheInfo == null)
            {
                viewModel = new AdministrationHomeViewModel();
                viewModel.Disciplines = this.disciplineService.GetAllDiscpline();
                viewModel.Candidacies = this.candidacyService.GetAllManagerCandidacy();

                await this.distributedCache.SetStringAsync("MemoryData", JsonConvert.SerializeObject(viewModel), new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(1 * 60),
                });
            }
            else
            {
                viewModel = JsonConvert.DeserializeObject<AdministrationHomeViewModel>(stringCacheInfo);
            }

            this.ViewData["CreateActionName"] = nameof(SchoolController).Replace("Controller", string.Empty) + "/" + nameof(SchoolController.AddManager);
            return this.View(viewModel);
        }

        [AdminManagerAuthorizeAttribute]
        public IActionResult SchoolManagerHome(int id)
        {
            this.ViewData["CreateActionName"] = nameof(SchoolController).Replace("Controller", string.Empty) + "/" + nameof(SchoolController.AddTeacher);
            this.ViewData["redirectUrl"] = id != 0 ? $"&redirectUrl=/Administration/Dashboard/{nameof(this.SchoolManagerHome)}?Id={id}"
                : $"&redirectUrl=/Administration/Dashboard/{nameof(this.SchoolManagerHome)}";

            if (id != 0)
            {
                var viewModel = this.schoolService.GetManagerHomePageBySchholId(id, this.environment.WebRootPath);
                return this.View(viewModel);
            }
            else
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var viewModel = this.schoolService.GetManagerHomePageData(userId, this.environment.WebRootPath);
                return this.View(viewModel);
            }
        }

        public IActionResult ShowCv(string id)
        {
            return this.PhysicalFile(this.environment.WebRootPath + $"/applicationDocuments/{id}.pdf", "application/pdf");
        }

        [AdminManagerAuthorize]
        public IActionResult StudentCandidacies(int schoolId, int page)
        {
            this.ViewData["redirectUrl"] = schoolId != 0 ? $"&redirectUrl=/Administration/Dashboard/{nameof(this.StudentCandidacies)}?SchoolId={schoolId}"
                : $"&redirectUrl=/Administration/Dashboard/{nameof(this.StudentCandidacies)}";

            if (schoolId == 0)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                schoolId = this.teacherService.GetSchoolId(userId);
            }

            this.ViewData["CreateActionName"] = nameof(TeamController).Replace("Controller", string.Empty) + "/" + nameof(TeamController.AddStudentToTeam);

            var viewModel = this.candidacyService.GetAllStudetnCandidacies(schoolId, page);
            return this.View(viewModel);
        }
    }
}
