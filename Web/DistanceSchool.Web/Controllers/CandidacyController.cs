namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Common;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Services.Data;
    using DistanceSchool.Services.Messaging;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Candidacyes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class CandidacyController : BaseController

    {
        private readonly ICandidacyServices candidacyServices;
        private readonly ISchoolService schoolService;
        private readonly ITeacherService teacherServisce;
        private readonly IWebHostEnvironment environment;
        private readonly ICustomEmailSenderService emailSender;

        public CandidacyController(
            ICandidacyServices candidacyServices,
            ISchoolService schoolService,
            ITeacherService teacherServisce,
            IWebHostEnvironment environment,
            ICustomEmailSenderService emailSender)
        {
            this.candidacyServices = candidacyServices;
            this.schoolService = schoolService;
            this.teacherServisce = teacherServisce;
            this.environment = environment;
            this.emailSender = emailSender;
        }

        [AdminManagerAuthorizeAttribute]
        public async Task<IActionResult> DeleteCandidacy(int id, string redirectUrl)
        {
            await this.candidacyServices.DeleteCandicayAsync(id);

            this.emailSender.DiapprovedUserSend(id);
            
            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                return this.Redirect("/");
            }

            return this.Redirect(redirectUrl);
        }

        [Authorize]
        public async Task<IActionResult> TeacherCandidacy(int id)
        {
            var inputModel = await this.CreateCandidacyForm(id, CandidacyType.Teacher);

            if (inputModel.IsAlreadyTeacher)
            {
                return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesTeacherCandidacy, inputModel.SchoolName));
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
            await this.candidacyServices.AddCandidacyAsync(inputModel, CandidacyType.Teacher, this.environment.WebRootPath);
            return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesTeacherCandidacy, schoolName));
        }

        [Authorize]
        public async Task<IActionResult> MangerCandidacy(int id)
        {
            var inputModel = await this.CreateCandidacyForm(id, CandidacyType.Manager);

            if (inputModel.IsAlreadyTeacher)
            {
                return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesMangerCandidacy, inputModel.SchoolName));
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
            await this.candidacyServices.AddCandidacyAsync(inputModel, CandidacyType.Manager, this.environment.WebRootPath);
            return this.RedirectToSucsessPage(string.Format(GlobalConstants.CyrillicSucssesMangerCandidacy, schoolName));
        }

        [Authorize]
        public async Task<IActionResult> StudentCandidacy(int id)
        {
            var inputModel = new StudentCandidacyInputModel
            {
                TeamId = id,
                CandidacyTypeMessage = GlobalConstants.CyrillicStudentCandicdacyHedarMessage,
                SchoolName = this.schoolService.GetSchoolNameByTeamId(id),
                BirthDate = DateTime.UtcNow.AddYears(-7),
                UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
            };

            if (await this.candidacyServices.AddAllreadyStudentCandidacyAsync(inputModel))
            {
                return this.RedirectToSucsessPage(string.Format(
                GlobalConstants.CyrillicSucssesStudentCandidacy,
                this.schoolService.GetSchoolNameByTeamId(inputModel.TeamId)));
            }

            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StudentCandidacy(StudentCandidacyInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.CandidacyTypeMessage = GlobalConstants.CyrillicStudentCandicdacyHedarMessage;
                inputModel.SchoolName = this.schoolService.GetSchoolNameByTeamId(inputModel.TeamId);
                return this.View(inputModel);
            }

            inputModel.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.candidacyServices.AddStudentCandidacyAsync(inputModel, this.environment.WebRootPath);

            return this.RedirectToSucsessPage(string.Format(
                GlobalConstants.CyrillicSucssesStudentCandidacy,
                this.schoolService.GetSchoolNameByTeamId(inputModel.TeamId)));
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
                await this.candidacyServices.AddCandidacyAsync(inputModel, candidacyType, this.environment.WebRootPath);
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
