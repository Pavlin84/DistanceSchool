namespace DistanceSchool.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Areas.Administration.Controllers;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Students;
    using DistanceSchool.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class TeamController : BaseController
    {
        private readonly ITeamService teamService;
        private readonly IDisciplineService disciplineService;

        public TeamController(ITeamService teamService, IDisciplineService disciplineService)
        {
            this.teamService = teamService;
            this.disciplineService = disciplineService;
        }

        [Authorize]
        public IActionResult OneTeam(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var team = this.teamService.GetTeamData(id, userId);

            var student = new StudentForOneTeamViewModel
            {
                Id = "studentId",
                SudentNames = "Pavlin Valeriew",
            };

            for (int i = 0; i < 15; i++)
            {
                team.Students.Add(student);
            }

            return this.View(team);
        }

        [AdminManagerAuthorizeAttribute]
        public IActionResult AddTeam(int id)
        {
            var viewModel = new AddTeamInputModel
            {
                TeamName = "Име на класа",
                SchoolId = id,
            };

            var disciplines = this.disciplineService.GetSchoolDisciplines(id);

            this.ViewBag.Disciplines = this.disciplineService.GetSchoolDisciplines(id).Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString(),
            });


            return this.View(viewModel);
        }

        [AdminManagerAuthorizeAttribute]
        [HttpPost]
        public async Task<IActionResult> AddTeam(AddTeamInputModel team)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var teamId = await this.teamService.AddTeam(team);

            return this.RedirectToAction("OneTeam", new { Id = teamId });
        }

        [AdminManagerAuthorize]
        public IActionResult AddDiscipline(int id)
        {
            var viewModel = this.disciplineService.GetAllDisciplineForTeam(id);
            return this.View(viewModel);
        }

        [HttpPost]
        [AdminManagerAuthorize]
        public async Task<IActionResult> AddDiscipline(AddDisciplinesToTeamInputModel inputModel)
        {
            await this.teamService.AddDiscipineToTeam(inputModel);

            return this.RedirectToAction(nameof(this.OneTeam), new { id = inputModel.TeamId });
        }

        [AdminManagerAuthorize]
        public async Task<IActionResult> AddStudentToTeam(int id)
        {
            var schoolId = await this.teamService.AddStudentToTeamAsync(id);
            var urlPatern = $"/Administration/{nameof(DashboardController).Replace("Controller", string.Empty)}/" +
                $"{nameof(DashboardController.StudentCandidacies)}?SchoolId={schoolId}";

            return this.Redirect(urlPatern);
        }
    }
}