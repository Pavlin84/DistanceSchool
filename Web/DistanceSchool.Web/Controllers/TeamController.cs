namespace DistanceSchool.Web.Controllers
{

    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Students;
    using DistanceSchool.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Mvc;

    public class TeamController : BaseController
    {
        public IActionResult OneTeam(string Id)
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

        public IActionResult AddTeam(string id)
        {
            var viewModel = new AddTeamInputModel
            {
                TeamName = "9",
                SchoolId = id,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult AddTeam(string teamName, string teamLevel)
        {
            var inputModel = new AddTeamInputModel
            {
                TeamName = teamName,
            };

            return this.RedirectToAction("OneTeam", new { Id = "newTeamId" });
        }

        public IActionResult AddDiscipline(string id)
        {
            return this.View();
        }
    }
}
