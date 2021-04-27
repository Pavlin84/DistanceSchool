namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DistanceSchool.Common;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.ViewModels.Candidacyes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CandidacyController : BaseController

    {
        private readonly ICandidacyServices candidacyServices;
        private readonly ISchoolService schoolService;
        private readonly ITeacherServisce teacherServisce;

        public CandidacyController(
            ICandidacyServices candidacyServices,
            ISchoolService schoolService,
            ITeacherServisce teacherServisce)
        {
            this.candidacyServices = candidacyServices;
            this.schoolService = schoolService;
            this.teacherServisce = teacherServisce;
        }

        // TO DO Only Administration Acsses
        public async Task<IActionResult> DelеteCandidacy(int id)
        {
            await this.candidacyServices.DeleteCandicayAsync(id);

            return this.Redirect("/Home/Administration");
        }

        [Authorize]
        public async Task<IActionResult> MangerCandidacy(int id)
        {
            var inputModel = await this.CreateCandidacyForm(id);

            if (inputModel.IsAlreadyTeacher)
            {
                return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesMangerCandidacy, inputModel.SchoolName, 0));
            }

            return this.View(inputModel);
        }

        [Authorize]
        public async Task<IActionResult> TeacherCandidacy(int id)
        {
            var inputModel = await this.CreateCandidacyForm(id);

            if (inputModel.IsAlreadyTeacher)
            {
                return this.RedirectToSucsessPage(GlobalConstants.CyrillicSucssesTeacherCandidacy);
            }

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MangerCandidacy(ManagerCandidacyInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = new ManagerCandidacyInputModel
                {
                    SchoolName = this.schoolService.GetSchoolName(inputModel.SchoolId),
                    BirthDate = inputModel.BirthDate,
                    SchoolId = inputModel.SchoolId,
                };
                return this.View(viewModel);
            }

            inputModel.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.candidacyServices.AddCandidacyAsync(inputModel);
            return this.RedirectToSucsessPage(GlobalConstants.CyrillicSucssesMangerCandidacy);
        }

        private IActionResult RedirectToSucsessPage(string message)
        {
            var controlerName = nameof(HomeController).Replace("Controller", string.Empty);
            var actionName = nameof(HomeController.Sucsess);

            return this.RedirectToAction(actionName, controlerName, new { message = message });
        }

        private async Task<ManagerCandidacyInputModel> CreateCandidacyForm(int id)
        {
            var schollName = this.schoolService.GetSchoolName(id);

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var inputModel = new ManagerCandidacyInputModel
            {
                SchoolId = id,
                SchoolName = schollName,
            };

            if (this.teacherServisce.IsTeacher(userId))
            {
                inputModel.UserId = userId;
                await this.candidacyServices.AddCandidacyAsync(inputModel);
                inputModel.IsAlreadyTeacher = true;
                return inputModel;
            }

            inputModel.BirthDate = DateTime.UtcNow.AddYears(-21);

            return inputModel;
        }
    }
}
