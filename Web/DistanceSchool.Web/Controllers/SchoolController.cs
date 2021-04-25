namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
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
        private readonly ITeacherServisce teacherServisce;

        public SchoolController(
            ISchoolService schoolService,
            ICandidacyServices candidacyServices,
            ITeacherServisce teacherServisce)
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
            school.IsUserManager = this.schoolService.IsUserManger(userId, id);

            return this.View(school);
        }

        // TODO Create constraint only for administrator
        public IActionResult AddSchool()
        {
            return this.View();
        }

        // TODO Create constraint only for administrator
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

        public async Task<IActionResult> AddManager(int id)
        {
            await this.schoolService.AddManagerAsync(id);

            return this.RedirectToAction(nameof(this.OneSchool), new { Id = id });
        }

        public async Task<IActionResult> RemoveManager(int id)
        {
            await this.schoolService.RemoveManagerAsync(id);

            return this.RedirectToAction(nameof(this.OneSchool), new { Id = id });
        }
    }
}
