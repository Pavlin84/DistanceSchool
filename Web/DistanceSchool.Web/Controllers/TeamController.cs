namespace DistanceSchool.Web.Controllers
{
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Students;
    using DistanceSchool.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class TeamController : BaseController
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public IActionResult OneTeam(string id)
        {
            var team = new OneTeamViewModel
            {
                Id = "teamId",
                TeamName = "11 A",
                SchoolName = " I ОУ Васил Левски",
            };

            var student = new StudentForOneTeamViewModel
            {
                Id = "studentId",
                SudentNames = "Pavlin Valeriew",
            };

            var discipline = new DisciplineForOneTeamViewModel
            {
                Id = "123",
                DisciplineName = "Математика",
                TeacherNames = "Галя Манева",
            };

            for (int i = 0; i < 15; i++)
            {
                team.Students.Add(student);
            }

            for (int i = 0; i < 5; i++)
            {
                team.Disciplines.Add(discipline);
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

        public IActionResult AddDiscipline(string id)
        {
            return this.View();
        }
    }
}
