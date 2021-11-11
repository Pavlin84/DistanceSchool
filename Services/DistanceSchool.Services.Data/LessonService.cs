namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Services.Mapping;
    using DistanceSchool.Web.ViewModels.Lessons;

    public class LessonService : ILessonService
    {
        private readonly IDeletableEntityRepository<TeacherTeam> teacherTeamRepository;
        private readonly IDeletableEntityRepository<Lesson> lessonRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IDeletableEntityRepository<Team> teamRepository;
        private readonly IDeletableEntityRepository<StudentLesson> studentLessonRepository;

        public LessonService(
            IDeletableEntityRepository<TeacherTeam> teacherTeamRepository,
            IDeletableEntityRepository<Lesson> lessonRepository,
            IFileHandlerService fileHandlerService,
            IDeletableEntityRepository<Team> teamRepository,
            IDeletableEntityRepository<StudentLesson> studentLessonRepository)
        {
            this.teacherTeamRepository = teacherTeamRepository;
            this.lessonRepository = lessonRepository;
            this.fileHandlerService = fileHandlerService;
            this.teamRepository = teamRepository;
            this.studentLessonRepository = studentLessonRepository;
        }

        public async Task AddLessonToTeam(int teamId, int lessonId, DateTime studyDate)
        {
            var studentsId = this.teamRepository.All().Where(x => x.Id == teamId)
                .Select(x => x.Students.Select(y => y.Id)).FirstOrDefault();

            foreach (var id in studentsId)
            {
                await this.studentLessonRepository.AddAsync(new StudentLesson
                {
                    StudentId = id,
                    LessonId = lessonId,
                    StudyDate = studyDate,
                    IsAttended = false,
                });
            }

            await this.studentLessonRepository.SaveChangesAsync();
        }

        public async Task CreateLessonAsync(string directoryPath, CreateAndAddLessonViewModel inputModel)
        {
            var lessonLevel = this.teamRepository.All().FirstOrDefault(x => x.Id == inputModel.TeamId).Level;

            var lesson = new Lesson
            {
                Name = inputModel.NewLessonName,
                DisciplineId = inputModel.DisciplineId,
                Level = lessonLevel,
                FilePath = Path.GetExtension(inputModel.NewLessonData.FileName),
            };

            await this.lessonRepository.AddAsync(lesson);
            await this.lessonRepository.SaveChangesAsync();

            var fileName = lesson.Id + lesson.FilePath;
            var lessonsDirectoryPath = $"{directoryPath}/Lessons";

            await this.fileHandlerService.SaveFileAsync(inputModel.NewLessonData, lessonsDirectoryPath, fileName);

            await this.AddLessonToTeam(inputModel.TeamId, lesson.Id, inputModel.DateOfStudy);
        }

        public T GetDisciplineLesonWithTeacherTeamId<T>(int teacherTeamId)
        {

            var viewModel = this.teacherTeamRepository.All()
                .Where(x => x.Id == teacherTeamId)
                .To<T>()
                .FirstOrDefault();

            return viewModel;
        }

    }
}
