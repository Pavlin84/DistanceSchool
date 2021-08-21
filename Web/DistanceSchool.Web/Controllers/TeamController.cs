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
        private readonly ITeacherService teacherService;
        private readonly IDisciplineService disciplineService;

        public TeamController(ITeamService teamService, ITeacherService teacherService, IDisciplineService disciplineService)
        {
            this.teamService = teamService;
            this.teacherService = teacherService;
            this.disciplineService = disciplineService;
        }

        [Authorize]
        public IActionResult OneTeam(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var team = this.teamService.GetTeamData(id, userId);

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
        public IActionResult AddDiscipline(int teamId)
        {
            var viewModel = this.disciplineService.GetAllDisciplineForTeam(teamId);
            return this.View(viewModel);
        }

        [HttpPost]
        [AdminManagerAuthorize]
        public async Task<IActionResult> AddDiscipline(AddDisciplinesToTeamInputModel inputModel)
        {
            await this.teamService.AddDiscipineToTeamAsync(inputModel);

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

        [AdminManagerAuthorize]
        public IActionResult ShiftsTecher(int teacherTeamId)
        {
            var viewModel = this.teacherService.ShiftsTeacher(teacherTeamId);

            return this.View(viewModel);
        }

        [HttpPost]
        [AdminManagerAuthorize]
        public async Task<IActionResult> ShiftsTecher(int teacherTeamId, string teacherId)
        {
            if (teacherId == null)
            {
                return this.Redirect($"/Team/ShiftsTecher?TeacherTeamId={teacherTeamId}");
            }

            await this.teamService.ChangeTeacher(teacherTeamId, teacherId);

            return this.Redirect($"/Teacher/OneTeacher?TeacherId={teacherId}");
        }
    }
}
