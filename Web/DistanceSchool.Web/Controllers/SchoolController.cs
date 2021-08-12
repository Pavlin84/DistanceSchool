namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Candidacyes;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Schools;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SchoolController : BaseController
    {
        private readonly ISchoolService schoolService;
        private readonly ICandidacyServices candidacyServices;
        private readonly ITeacherService teacherServisce;

        public SchoolController(
            ISchoolService schoolService,
            ICandidacyServices candidacyServices,
            ITeacherService teacherServisce)
        {
            this.schoolService = schoolService;
            this.candidacyServices = candidacyServices;
            this.teacherServisce = teacherServisce;
        }

        public IActionResult AllSchool()
        {
            var viewModel = this.schoolService.AllSchool();

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult OneSchool(int id)
        {
            var school = this.schoolService.GetSchoolData(id);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            school.IsUserManager = this.schoolService.IsUserMangerToSchool(userId, id);
            school.IsTeacherInSchool = this.teacherServisce.IsUserTeacherToSchool(userId, id);

            return this.View(school);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult AddSchool()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddSchool(AddSchoolInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.schoolService.AddSchoolAsync(inputModel);

            return this.RedirectToAction(nameof(this.AllSchool));
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> AddManager(int id)
        {
           var schoolId = await this.schoolService.AddManagerAsync(id);

           return this.RedirectToAction(nameof(this.OneSchool), new { Id = schoolId });
        }

        [AdminManagerAuthorizeAttribute]
        public async Task<IActionResult> AddTeacher(int id)
        {
            var teacherId = await this.schoolService.AddTeacherToSchool(id);

            return this.RedirectToAction(nameof(TeacherController.OneTeacher), nameof(TeacherController).Replace("Controller", string.Empty), new { teacherId = teacherId });
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> RemoveManager(int id)
        {
            await this.schoolService.RemoveManagerAsync(id);

            return this.RedirectToAction(nameof(this.OneSchool), new { Id = id });
        }
    }
}
