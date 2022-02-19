namespace DistanceSchool.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Exams;
    using DistanceSchool.Web.ViewModels.Lessons;
    using DistanceSchool.Web.ViewModels.Students;
    using DistanceSchool.Web.ViewModels.Teams;

    public class TeamService : ITeamService
    {
        private readonly IDeletableEntityRepository<Team> teamsRepository;
        private readonly IDisciplineService disciplineService;
        private readonly ISchoolService schoolService;
        private readonly IDeletableEntityRepository<TeacherTeam> teacherTeamsRepository;
        private readonly IDeletableEntityRepository<Student> studentRepository;
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;
        private readonly IRepository<StudentLesson> studentLesonRepository;
        private readonly IRepository<StudentExam> studentExamRepository;
        private readonly ICandidacyServices candidacyServices;
        private readonly IStudentService studentService;
        private readonly IDeletableEntityRepository<School> schoolRepository;

        public TeamService(
            IDeletableEntityRepository<Team> teamsRepository,
            IDisciplineService disciplineService,
            ISchoolService schoolService,
            IDeletableEntityRepository<TeacherTeam> teacherTeamsRepository,
            IDeletableEntityRepository<Student> studentRepository,
            IDeletableEntityRepository<Candidacy> candidacyRepository,
            IRepository<StudentLesson> studentLesonRepository,
            IRepository<StudentExam> studentExamRepository,
            ICandidacyServices candidacyServices,
            IStudentService studentService,
            IDeletableEntityRepository<School> schoolRepository)
        {
            this.teamsRepository = teamsRepository;
            this.disciplineService = disciplineService;
            this.schoolService = schoolService;
            this.teacherTeamsRepository = teacherTeamsRepository;
            this.studentRepository = studentRepository;
            this.candidacyRepository = candidacyRepository;
            this.studentLesonRepository = studentLesonRepository;
            this.studentExamRepository = studentExamRepository;
            this.candidacyServices = candidacyServices;
            this.studentService = studentService;
            this.schoolRepository = schoolRepository;
        }

        public async Task AddDiscipineToTeamAsync(AddDisciplinesToTeamInputModel inputModel)
        {
            var disciplines = this.teamsRepository.All().Where(x => x.Id == inputModel.TeamId).Select(x => x.TeacherTeams).FirstOrDefault();

            foreach (var discipline in disciplines)
            {
                if (!inputModel.DisciplinesId.Any(x => x.Equals(discipline.DisciplineId)))
                {
                    discipline.IsDeleted = true;
                }
            }

            foreach (var disciplineId in inputModel.DisciplinesId)
            {
                if (!disciplines.Any(x => x.DisciplineId == disciplineId))
                {
                    await this.teacherTeamsRepository.AddAsync(new TeacherTeam { TeamId = inputModel.TeamId, DisciplineId = disciplineId });
                }
            }

            await this.teamsRepository.SaveChangesAsync();
            await this.teacherTeamsRepository.SaveChangesAsync();
        }

        public async Task<int> AddStudentToTeamAsync(int id)
        {
            var studentId = this.candidacyRepository.All()
              .Where(x => x.Id == id)
              .Select(x => x.ApplicationUser)
              .FirstOrDefault()
              .StudentId;

            if (studentId == null)
            {
                studentId = await this.studentService.CreateNewStudentAsync(id);
            }

            var student = this.studentRepository.All().FirstOrDefault(x => x.Id == studentId);

            var candidacy = this.candidacyRepository.All().FirstOrDefault(x => x.Id == id);

            student.TeamId = (int)candidacy.TeamId;

            // TO DO Check thii when implement Lessons And Exams
            var studentExams = this.studentExamRepository.All().Where(x => x.StudentId == studentId);
            var studentLessons = this.studentLesonRepository.All().Where(x => x.StudentId == studentId);
            foreach (var exam in studentExams)
            {
                exam.IsDeleted = true;
            }

            foreach (var lesson in studentLessons)
            {
                lesson.IsDeleted = true;
            }

            await this.studentRepository.SaveChangesAsync();
            await this.studentExamRepository.SaveChangesAsync();
            await this.studentLesonRepository.SaveChangesAsync();

            await this.candidacyServices.DeleteCandicayAsync(id);

            return (int)candidacy.SchoolId;
        }

        public async Task<int> AddTeam(AddTeamInputModel team)
        {
            var newTeam = new Team
            {
                Name = team.TeamName,
                SchoolId = team.SchoolId,
                Level = team.TeamLevel,
            };
            if (team.DisciplinesId != null)
            {
                foreach (var disciplineId in team.DisciplinesId)
                {
                    newTeam.TeacherTeams.Add(new TeacherTeam { DisciplineId = disciplineId });
                }
            }

            await this.teamsRepository.AddAsync(newTeam);

            await this.teamsRepository.SaveChangesAsync();

            return newTeam.Id;
        }

        public async Task ChangeTeacher(int teacherTeamId, string teacherId)
        {
            this.teacherTeamsRepository.All().FirstOrDefault(x => x.Id == teacherTeamId).TeacherId = teacherId;

            await this.teacherTeamsRepository.SaveChangesAsync();
        }

        public List<MatchedTeamsViewModel> GetAllMatchedTeams(List<int> disciplinesId)
        {
            var teamsQuery = this.teamsRepository.All().AsQueryable();

            foreach (var disciplineId in disciplinesId)
            {
                teamsQuery = teamsQuery.Where(x => x.TeacherTeams.Any(y => y.DisciplineId == disciplineId));
            }

            var viewModel = teamsQuery.Select(x => new MatchedTeamsViewModel
            {
                TeamId = x.Id,
                TeamName = x.Name + " " + x.Level,
                SchoolName = x.School.Name,
                MatchedDisciplines = x.TeacherTeams.Where(y => disciplinesId
                      .Contains(y.DisciplineId))
                      .Select(y => y.Discipline.Name).ToList(),
                NotMatchedDisciplines = x.TeacherTeams.Where(y => !disciplinesId
                      .Contains(y.DisciplineId))
                      .Select(y => y.Discipline.Name).ToList(),
            }).ToList();

            return viewModel;
        }

        public List<MatchedTeamsViewModel> GetMatchedTeams(List<int> disciplinesId)
        {
            var viewModel = this.teamsRepository.All()
                .Where(x => x.TeacherTeams.Any(y => disciplinesId.Contains(y.DisciplineId)))
                .Select(x => new MatchedTeamsViewModel
                {
                    TeamId = x.Id,
                    TeamName = x.Name + " " + x.Level,
                    SchoolName = x.School.Name,
                    MatchedDisciplines = x.TeacherTeams.Where(y => disciplinesId
                          .Contains(y.DisciplineId))
                          .Select(y => y.Discipline.Name).ToList(),
                    NotMatchedDisciplines = x.TeacherTeams.Where(y => !disciplinesId
                          .Contains(y.DisciplineId))
                          .Select(y => y.Discipline.Name).ToList(),
                }).ToList();

            return viewModel;
        }

        public OneTeamViewModel GetTeamData(int id, string userId)
        {
            var team = this.teamsRepository.All().Where(x => x.Id == id)
                .Select(x => new OneTeamViewModel
                {
                    Id = x.Id,
                    TeamName = x.Level + " " + x.Name,
                    SchoolName = x.School.Name,
                    Disciplines = this.disciplineService.GetTeamDisciplines(id),
                    IsUserManager = this.schoolRepository.All().FirstOrDefault(y => y.Id == x.SchoolId).ManagerId == userId,
                    IsTeachesToTeam = x.TeacherTeams.Any(y => y.Teacher.ApplicationUserId == userId),
                    IsStudiesToTeam = x.Students.Any(y => y.ApplicationUserId == userId),
                    Students = x.Students.Select(y => new StudentForOneTeamViewModel
                    {
                        Id = y.Id,
                        SudentNames = y.FirstName + " " + y.LastName,
                    }).ToList(),
                }).FirstOrDefault();

            return team;
        }

        public TeamPassportVewModel GetTeamPassportData(int teacherTeamId)
        {
            var disciplineId = this.teacherTeamsRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == teacherTeamId).DisciplineId;
            var level = this.teacherTeamsRepository.AllAsNoTracking()
                .Where(x => x.Id == teacherTeamId)
                .Select(t => t.Team).FirstOrDefault()
                .Level;

            var viewModel = this.teamsRepository.All()
                .Where(x => x.TeacherTeams.Any(y => y.Id == teacherTeamId))
                .Select(x => new TeamPassportVewModel
                {
                    Id = x.Id,
                    TeamName = x.Name + " " + x.Level,
                    Lessons = x.Students.FirstOrDefault().StudentLessons
                        .Where(l => l.Lesson.DisciplineId == disciplineId && l.Lesson.Level == x.Level)
                        .OrderBy(l => l.StudyDate)
                        .Select(l => l.Lesson.Name)
                        .ToList(),
                    TeacherTheamId = teacherTeamId,
                    Exam = this.studentExamRepository.All()
                        .Where(x => x.Student.TeamId == x.Id)
                        .Select(x => new ExamViewModel
                        {
                            Id = x.ExamId,
                            StartDateTime = x.ExamDate,
                        }).FirstOrDefault(),
                    Discipline = x.TeacherTeams.FirstOrDefault(y => y.Id == teacherTeamId).Discipline.Name,
                    Students = x.Students.Select(y => new StudentForTeamPassportViewModel
                    {
                        Id = y.Id,
                        SudentNames = y.FirstName + " " + y.LastName,
                        Exam = this.studentExamRepository.All()
                            .Where(x => x.StudentId == y.Id)
                            .Select(x => new ExamViewModel
                            {
                                Id = x.ExamId,
                                StartDateTime = x.ExamDate,
                                Evaluation = x.Evaluation,
                            }).FirstOrDefault(),
                        Lessons = y.StudentLessons
                            .Where(l => l.Lesson.DisciplineId == disciplineId && l.Lesson.Level == level)
                            .Select(z => new LessonViewModel
                            {
                                Id = z.LessonId,
                                Name = z.Lesson.Name,
                                DateOfStudy = z.StudyDate,
                                IsStudied = z.IsAttended,
                            }).OrderBy(l => l.DateOfStudy).ToList(),
                    }).ToList(),
                }).FirstOrDefault();

            return viewModel;
        }

        private ExamViewModel GetExamByStudentId(string id)
        {
            var viewModel = this.studentExamRepository.All()
                 .Where(x => x.StudentId == id)
                 .Select(x => new ExamViewModel
                 {
                     Id = x.ExamId,
                     StartDateTime = x.ExamDate,
                     Evaluation = x.Evaluation,
                 }).FirstOrDefault();

            return viewModel;
        }

        private ExamViewModel GetExamByTeamId(int id)
        {
            var viewModel = this.studentExamRepository.All()
                 .Where(x => x.Student.TeamId == id)
                 .Select(x => new ExamViewModel
                 {
                     Id = x.ExamId,
                     StartDateTime = x.ExamDate,
                     Evaluation = x.Evaluation,
                 }).FirstOrDefault();

            return viewModel;
        }
    }
}
