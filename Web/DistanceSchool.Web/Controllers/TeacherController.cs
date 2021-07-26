namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Mvc;

    public class TeacherController : BaseController
    {
        [TeacherProfilAccess]
        public IActionResult OneTeacher(string id)
        {

            var inputModel = new OneTeachetViewModel
            {
                Id = id,
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

            return this.View(inputModel);
        }
    }
}
