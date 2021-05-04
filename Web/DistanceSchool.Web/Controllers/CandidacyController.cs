namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DistanceSchool.Common;
    using DistanceSchool.Data.Models;
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
        public async Task<IActionResult> TeacherCandidacy(int id)
        {
            var inputModel = await this.CreateCandidacyForm(id, CandidacyType.Teacher);

            if (inputModel.IsAlreadyTeacher)
            {
                return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesTeacherCandidacy, inputModel.SchoolName, 0));
            }

            inputModel.CandidacyTypeMessage = GlobalConstants.CyrillicTeachererCandicdacyHedarMessage;

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TeacherCandidacy(TeacherCandidacyInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                TeacherCandidacyInputModel viewModel = this.CreateCandidacyFormViewModel(inputModel);
                return this.View(viewModel);
            }

            var schoolName = this.schoolService.GetSchoolName(inputModel.SchoolId);

            inputModel.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.candidacyServices.AddCandidacyAsync(inputModel, CandidacyType.Teacher);
            return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesTeacherCandidacy, schoolName, 0));
        }

        [Authorize]
        public async Task<IActionResult> MangerCandidacy(int id)
        {
            var inputModel = await this.CreateCandidacyForm(id, CandidacyType.Manager);

            if (inputModel.IsAlreadyTeacher)
            {
                return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesMangerCandidacy, inputModel.SchoolName, 0));
            }

            inputModel.CandidacyTypeMessage = GlobalConstants.CyrillicMangerCandicdacyHedarMessage;

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MangerCandidacy(TeacherCandidacyInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                TeacherCandidacyInputModel viewModel = this.CreateCandidacyFormViewModel(inputModel);
                return this.View(viewModel);
            }

            var schoolName = this.schoolService.GetSchoolName(inputModel.SchoolId);

            inputModel.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.candidacyServices.AddCandidacyAsync(inputModel, CandidacyType.Manager);
            return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesMangerCandidacy, schoolName, 0));
        }

        private IActionResult RedirectToSucsessPage(string message)
        {
            var controlerName = nameof(HomeController).Replace("Controller", string.Empty);
            var actionName = nameof(HomeController.Sucsess);

            return this.RedirectToAction(actionName, controlerName, new { message = message });
        }

        private async Task<TeacherCandidacyInputModel> CreateCandidacyForm(int id, CandidacyType candidacyType)
        {
            var schollName = this.schoolService.GetSchoolName(id);

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var inputModel = new TeacherCandidacyInputModel
            {
                SchoolId = id,
                SchoolName = schollName,
            };

            if (this.teacherServisce.IsTeacher(userId))
            {
                inputModel.UserId = userId;
                await this.candidacyServices.AddCandidacyAsync(inputModel, candidacyType);
                inputModel.IsAlreadyTeacher = true;
                return inputModel;
            }

            inputModel.BirthDate = DateTime.UtcNow.AddYears(-21);

            return inputModel;
        }

        private TeacherCandidacyInputModel CreateCandidacyFormViewModel(TeacherCandidacyInputModel inputModel)
        {
            return new TeacherCandidacyInputModel
            {
                SchoolName = this.schoolService.GetSchoolName(inputModel.SchoolId),
                BirthDate = inputModel.BirthDate,
                SchoolId = inputModel.SchoolId,
            };
        }
    }
}
