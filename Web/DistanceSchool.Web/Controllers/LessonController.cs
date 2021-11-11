namespace DistanceSchool.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Services.Data;
    using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
    using DistanceSchool.Web.ViewModels.Lessons;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class LessonController : BaseController
    {
        private readonly ILessonService lessonService;
        private readonly IWebHostEnvironment environment;

        public LessonController(ILessonService lessonService, IWebHostEnvironment environment)
        {
            this.lessonService = lessonService;
            this.environment = environment;
        }

        [TeamPassportAcsses]
        public IActionResult AddLessonToTeam(int teacherTheamId)
        {
            AddLessonViewModel viewModel = this.lessonService.GetDisciplineLesonWithTeacherTeamId<AddLessonViewModel>(teacherTheamId);

            viewModel.DateOfStudy = DateTime.UtcNow.AddDays(1);

            return this.View(viewModel);
        }

        [HttpPost]
        [TeamPassportAcsses]
        public async Task<IActionResult> AddLessonToTeam(AddLessonViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel = this.lessonService.GetDisciplineLesonWithTeacherTeamId<AddLessonViewModel>(inputModel.Id);

                return this.View(inputModel);
            }

            await this.lessonService.AddLessonToTeam(inputModel.TeamId, inputModel.Lesson, inputModel.DateOfStudy);

            return this.Redirect("/Team/Passport?TeacherTheamId=" + inputModel.Id);
        }

        [TeamPassportAcsses]
        public IActionResult CreateLesson(int teacherTheamId)
        {
            var viewModel = this.lessonService.GetDisciplineLesonWithTeacherTeamId<CreateAndAddLessonViewModel>(teacherTheamId);

            viewModel.DateOfStudy = DateTime.UtcNow.AddDays(1);

            return this.View(viewModel);
        }

        [HttpPost]
        [TeamPassportAcsses]
        public async Task<IActionResult> CreateLesson(CreateAndAddLessonViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                viewModel = this.lessonService.GetDisciplineLesonWithTeacherTeamId<CreateAndAddLessonViewModel>(viewModel.Id);

                return this.View(viewModel);
            }

            await this.lessonService.CreateLessonAsync(this.environment.WebRootPath, viewModel);

            return this.Redirect("/Team/Passport?TeacherTheamId=" + viewModel.Id);
        }
    }
}
