namespace DistanceSchool.Services.Data.Tests
{
    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teams;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class TeamServiceTest
    {
        [Fact]
        public async Task CheckAddDisciplineToTeamIsChangeTheDisciplines()
        {
            // Arrange
            var teamsRepository = new Mock<IDeletableEntityRepository<Team>>();
            var disciplineService = new Mock<IDisciplineService>();
            var schoolService = new Mock<ISchoolService>();
            var teacherTeamsRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var studentLesonRepository = new Mock<IDeletableEntityRepository<StudentLesson>>();
            var studentExamRepository = new Mock<IDeletableEntityRepository<StudentExam>>();
            var candidacyServices = new Mock<ICandidacyServices>();
            var studentService = new Mock<IStudentService>();

            var teacherTeamRepo = new List<TeacherTeam>();

            var teamsRepo = new List<Team>
            {
                new Team
                {
                    Id = 1,
                    TeacherTeams = new List<TeacherTeam>
                    {
                        new TeacherTeam
                        {
                            DisciplineId = 11,
                        },
                        new TeacherTeam
                        {
                            DisciplineId = 21,
                        },
                    },
                },
            };

            var teamService = new TeamService(
                teamsRepository.Object,
                disciplineService.Object,
                schoolService.Object,
                teacherTeamsRepository.Object,
                studentRepository.Object,
                candidacyRepository.Object,
                studentLesonRepository.Object,
                studentExamRepository.Object,
                candidacyServices.Object,
                studentService.Object);

            teacherTeamsRepository.Setup(r => r.AddAsync(It.IsAny<TeacherTeam>()))
                .Callback((TeacherTeam teacherTeam) => teacherTeamRepo.Add(teacherTeam));
            teamsRepository.Setup(r => r.All()).Returns(teamsRepo.AsQueryable());

            var inputModel = new AddDisciplinesToTeamInputModel
            {
                TeamId = 1,
                DisciplinesId = new List<int>
                {
                    21,
                    31,
                },
            };

            var teacherTeam = teamsRepo[0].TeacherTeams.ToList();

            // Act
            await teamService.AddDiscipineToTeamAsync(inputModel);

            // Assert
            Assert.True(teacherTeam[0].IsDeleted);
            Assert.Equal(21, teacherTeam[1].DisciplineId);
            Assert.True(teacherTeamRepo.Count == 2);
        }

        [Fact]
        public async Task CheckAddStudentTeamAddToNewTeam()
        {
            // Arrange
            var teamsRepository = new Mock<IDeletableEntityRepository<Team>>();
            var disciplineService = new Mock<IDisciplineService>();
            var schoolService = new Mock<ISchoolService>();
            var teacherTeamsRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var studentLesonRepository = new Mock<IDeletableEntityRepository<StudentLesson>>();
            var studentExamRepository = new Mock<IDeletableEntityRepository<StudentExam>>();
            var candidacyServices = new Mock<ICandidacyServices>();
            var studentService = new Mock<IStudentService>();

            var studentRepo = new List<Student>
            {
               new Student
               {
                   Id = "11",
                   TeamId = 3,
               },
               new Student
               {
                   Id = "15",
                   TeamId = 3,
               },
            };

            var candidacyRepo = new List<Candidacy>
            {
                new Candidacy
                {
                    Id = 1,
                    TeamId = 1111,
                    SchoolId = 0,
                    ApplicationUser = new ApplicationUser
                    {
                        StudentId = "11",
                    },
                },
                new Candidacy
                {
                    Id = 7,
                    TeamId = 1111,
                    ApplicationUser = new ApplicationUser
                    {
                        StudentId = "15",
                    },
                },
            };

            var teamService = new TeamService(
                teamsRepository.Object,
                disciplineService.Object,
                schoolService.Object,
                teacherTeamsRepository.Object,
                studentRepository.Object,
                candidacyRepository.Object,
                studentLesonRepository.Object,
                studentExamRepository.Object,
                candidacyServices.Object,
                studentService.Object);

            studentRepository.Setup(r => r.AddAsync(It.IsAny<Student>()))
                .Callback((Student student) => studentRepo.Add(student));
            studentRepository.Setup(r => r.All()).Returns(studentRepo.AsQueryable());
            candidacyRepository.Setup(r => r.All()).Returns(candidacyRepo.AsQueryable());

            // Act
            await teamService.AddStudentToTeamAsync(1);

            // Assert
            Assert.Equal(1111, studentRepo[0].TeamId);
            Assert.Equal(3, studentRepo[1].TeamId);
        }

        [Fact]
        public async Task CheckAddTeamIsCreatedNewTeam()
        {
            // Arrange
            var teamRepository = new Mock<IDeletableEntityRepository<Team>>();
            var disciplineService = new Mock<IDisciplineService>();
            var schoolService = new Mock<ISchoolService>();
            var teacherTeamsRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var studentLesonRepository = new Mock<IDeletableEntityRepository<StudentLesson>>();
            var studentExamRepository = new Mock<IDeletableEntityRepository<StudentExam>>();
            var candidacyServices = new Mock<ICandidacyServices>();
            var studentService = new Mock<IStudentService>();

            var teamRepo = new List<Team>();

            var teamService = new TeamService(
                teamRepository.Object,
                disciplineService.Object,
                schoolService.Object,
                teacherTeamsRepository.Object,
                studentRepository.Object,
                candidacyRepository.Object,
                studentLesonRepository.Object,
                studentExamRepository.Object,
                candidacyServices.Object,
                studentService.Object);

            teamRepository.Setup(r => r.All()).Returns(teamRepo.AsQueryable());
            teamRepository.Setup(r => r.AddAsync(It.IsAny<Team>()))
                .Callback((Team team) => teamRepo.Add(team));

            var inputModel = new AddTeamInputModel()
            {
                TeamName = "test",
                SchoolId = 11,
                DisciplinesId = new List<int>
                {
                    1,
                    2,
                    3,
                },
            };

            // Act
            await teamService.AddTeam(inputModel);

            // Assert
            Assert.True(teamRepo.Count == 1);
            Assert.True(teamRepo[0].TeacherTeams.Count == 3);

        }
    }
}
