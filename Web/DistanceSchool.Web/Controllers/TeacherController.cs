namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Mvc;

    public class TeacherController : BaseController
    {
        private readonly ITeacherService teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        [TeacherProfilAccess]
        public IActionResult OneTeacher(string teacherId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var inputModel = this.teacherService.GetTeacherData(teacherId);

            var inputModelTest = new OneTeacherViewModel
            {
                Id = teacherId,
                TeacherNames = "Ivan Spasov",
                SchoolName = "ОУ Васил Левски",
                Disciplines = new List<DisciplinesForOneTeacherViewModel>
                {
                    new DisciplinesForOneTeacherViewModel
                    {
                        Name = "Geography",
                        Teams = new List<TeamBaseViewModel>
                        {
                            new TeamBaseViewModel { Id = 1 , TeamName = "7a", },
                            new TeamBaseViewModel { Id = 56, TeamName = "8a", },
                            new TeamBaseViewModel { Id = 98, TeamName = "12l", },
                        },
                    },
                    new DisciplinesForOneTeacherViewModel
                    {
                        Name = "Music",
                        Teams = new List<TeamBaseViewModel>
                        {
                            new TeamBaseViewModel { Id = 1, TeamName = "6a", },
                            new TeamBaseViewModel { Id = 56, TeamName = "8c", },
                            new TeamBaseViewModel { Id = 98, TeamName = "12x", },
                        },
                    },
                },
            };

            return this.View(inputModelTest);
        }

        public IActionResult AddDiscipline(string teacherId)
        {
            var viewModel = this.teacherService.GetTeacherDisciplines(teacherId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscipline(AddDisciplineToTeacherInputModel inputModel)
        {
            await this.teacherService.AddDisciplinesToTeacherAsync(inputModel);

            return this.Redirect($"/{this.GetType().Name.Replace("Controller", string.Empty)}" +
                $"/{nameof(TeacherController.OneTeacher)}?TeacherId={inputModel.TeacherId}");
        }
    }
}
