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

            var inputModel = this.teacherService.GetTeacherData(teacherId, userId);

            return this.View(inputModel);
        }

        [AdminManagerAuthorize]
        public IActionResult AddDiscipline(string teacherId)
        {
            var viewModel = this.teacherService.GetTeacherDisciplines(teacherId);
            return this.View(viewModel);
        }

        [HttpPost]
        [AdminManagerAuthorize]
        public async Task<IActionResult> AddDiscipline(AddDisciplineToTeacherInputModel inputModel)
        {
            await this.teacherService.AddDisciplinesToTeacherAsync(inputModel);

            return this.Redirect($"/{this.GetType().Name.Replace("Controller", string.Empty)}" +
                $"/{nameof(TeacherController.OneTeacher)}?TeacherId={inputModel.TeacherId}");
        }
    }
}
