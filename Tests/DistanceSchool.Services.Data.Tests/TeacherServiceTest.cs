namespace DistanceSchool.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using Moq;
    using Xunit;

    public class TeacherServiceTest
    {
        [Fact]
        public async Task CheckCreateTeacherAsyncIsCreateTeacherIfIsNewTeacher()
        {
            // Arrange
            var teacherRepository = new Mock<IDeletableEntityRepository<Teacher>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineTeacherRepository = new Mock<IDeletableEntityRepository<DisciplineTeacher>>();
            var teacherTeamRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();

            var candidacyRepo = new List<Candidacy>
            {
                new Candidacy
                {
                    Id = 1,
                    SchoolId = 11,
                    FirstName = " ",
                    SecondName = " ",
                    LastName = " ",
                    BirthDate = DateTime.Now,
                    ApplicationUserId = "a",
                    ApplicationUser = new ApplicationUser
                    {
                        Id = "a",
                        TeacherId = null,
                    },
                },
                new Candidacy
                {
                    Id = 2,
                    SchoolId = 22,
                    FirstName = " ",
                    SecondName = " ",
                    LastName = " ",
                    BirthDate = DateTime.Now,
                    ApplicationUserId = "2a",
                    ApplicationUser = new ApplicationUser
                    {
                        Id = "2a",
                        TeacherId = null,
                    },

                },
            };
            var teacherRepo = new List<Teacher>();

            var teacherService = new TeacherService(
                teacherRepository.Object,
                candidacyRepository.Object,
                schoolRepository.Object,
                disciplineTeacherRepository.Object,
                teacherTeamRepository.Object);

            candidacyRepository.Setup(r => r.All()).Returns(candidacyRepo.AsQueryable());
            teacherRepository.Setup(r => r.AddAsync(It.IsAny<Teacher>())).Callback((Teacher teacher) => teacherRepo.Add(teacher));

            // Act
            await teacherService.CreateTeacherAsync(1);

            // Assert
            Assert.True(teacherRepo.Count == 1);
            Assert.True(teacherRepo[0].SchoolId == 11);
            Assert.True(teacherRepo[0].ApplicationUserId == "a");
        }

        [Fact]
        public async Task CheckChangeSchoolIdAsyncSwichSchoolIdAndDeleteTeamTeacherId()
        {
            // Arrange
            var teacherRepository = new Mock<IDeletableEntityRepository<Teacher>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineTeacherRepository = new Mock<IDeletableEntityRepository<DisciplineTeacher>>();
            var teacherTeamRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();

            var teacherRepo = new List<Teacher>
            {
                new Teacher
                {
                    SchoolId = 1,
                    ApplicationUserId = "11",
                },
            };
            var teacherTeamRepo = new List<TeacherTeam>
            {
                new TeacherTeam
                {
                    TeacherId = "test",
                    Teacher = new Teacher
                    {
                        ApplicationUserId = "11",
                    },
                },
                new TeacherTeam
                {
                    TeacherId = "test",
                    Teacher = new Teacher
                    {
                        ApplicationUserId = "11",
                    },
                },
                new TeacherTeam
                {
                    TeacherId = "test",
                    Teacher = new Teacher
                    {
                        ApplicationUserId = "21",
                    },
                },
            };

            var teacherService = new TeacherService(
                teacherRepository.Object,
                candidacyRepository.Object,
                schoolRepository.Object,
                disciplineTeacherRepository.Object,
                teacherTeamRepository.Object);

            teacherRepository.Setup(r => r.All()).Returns(teacherRepo.AsQueryable());
            teacherTeamRepository.Setup(r => r.All()).Returns(teacherTeamRepo.AsQueryable());

            // Act
            await teacherService.ChangeSchoolIdAsync("11", 222);

            // Assert
            Assert.Equal(222, teacherRepo[0].SchoolId);
            Assert.True(teacherTeamRepo[0].TeacherId == null);
            Assert.True(teacherTeamRepo[1].TeacherId == null);
            Assert.True(teacherTeamRepo[2].TeacherId == "test");

        }

        [Fact]
        public async Task CheckAddDisciplinesToTeacherAsyncChangeTeachreDisciplines()
        {
            // Arrange
            var teacherRepository = new Mock<IDeletableEntityRepository<Teacher>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineTeacherRepository = new Mock<IDeletableEntityRepository<DisciplineTeacher>>();
            var teacherTeamRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();

            var disciplineTeacherRepo = new List<DisciplineTeacher>
            {
                new DisciplineTeacher
                {
                    TeacherId = "test",
                    DisciplineId = 2,
                },
            };
            var teacherTeamRepo = new List<TeacherTeam>
            {
                new TeacherTeam
                {
                    TeacherId = "test",
                    TeamId = 1,
                    DisciplineId = 2,
                },
                new TeacherTeam
                {
                    TeacherId = "test2",
                    TeamId = 2,
                    DisciplineId = 2,
                },
            };

            var teacherService = new TeacherService(
                teacherRepository.Object,
                candidacyRepository.Object,
                schoolRepository.Object,
                disciplineTeacherRepository.Object,
                teacherTeamRepository.Object);

            disciplineTeacherRepository.Setup(r => r.All()).Returns(disciplineTeacherRepo.AsQueryable());
            disciplineTeacherRepository.Setup(r => r.AddAsync(It.IsAny<DisciplineTeacher>()))
                .Callback((DisciplineTeacher disciplineTeacher) => disciplineTeacherRepo.Add(disciplineTeacher));
            teacherTeamRepository.Setup(r => r.All()).Returns(teacherTeamRepo.AsQueryable());

            var inputModel = new AddDisciplineToTeacherInputModel
            {
                TeacherId = "test",
                DisciplinesId = new List<int>
                    {
                        1,
                        3,
                    },
            };

            // Act
            await teacherService.AddDisciplinesToTeacherAsync(inputModel);

            // Assert
            Assert.True(disciplineTeacherRepo[0].IsDeleted);
            Assert.Equal(1, disciplineTeacherRepo[1].DisciplineId);
            Assert.Equal(3, disciplineTeacherRepo[2].DisciplineId);
        }

        [Fact]
        public async Task CheckAddDisciplinesToTeacherAsyncDelteteTeacherIdFromTheyTeams()
        {
            // Arrange
            var teacherRepository = new Mock<IDeletableEntityRepository<Teacher>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineTeacherRepository = new Mock<IDeletableEntityRepository<DisciplineTeacher>>();
            var teacherTeamRepository = new Mock<IDeletableEntityRepository<TeacherTeam>>();

            var disciplineTeacherRepo = new List<DisciplineTeacher>
            {
                new DisciplineTeacher
                {
                    TeacherId = "test",
                    DisciplineId = 2,
                },
            };
            var teacherTeamRepo = new List<TeacherTeam>
            {
                new TeacherTeam
                {
                    TeacherId = "test",
                    TeamId = 1,
                    DisciplineId = 2,
                },
                new TeacherTeam
                {
                    TeacherId = "test2",
                    TeamId = 2,
                    DisciplineId = 2,
                },
            };

            var teacherService = new TeacherService(
                teacherRepository.Object,
                candidacyRepository.Object,
                schoolRepository.Object,
                disciplineTeacherRepository.Object,
                teacherTeamRepository.Object);

            disciplineTeacherRepository.Setup(r => r.All()).Returns(disciplineTeacherRepo.AsQueryable());
            teacherTeamRepository.Setup(r => r.All()).Returns(teacherTeamRepo.AsQueryable());

            var inputModel = new AddDisciplineToTeacherInputModel
            {
                TeacherId = "test",
                DisciplinesId = new List<int>
                    {
                        1,
                        3,
                    },
            };

            // Act
            await teacherService.AddDisciplinesToTeacherAsync(inputModel);

            // Assert
            Assert.Null(teacherTeamRepo[0].TeacherId);
            Assert.NotNull(teacherTeamRepo[1].TeacherId);
        }
    }
}
