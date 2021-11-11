namespace DistanceSchool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using Moq;
    using Xunit;

    public class DisciplineServiceTest
    {
        [Fact]
        public async Task CheckCreateDisciplineAsyncIsCreateDiscioline()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineRepository = new Mock<IDeletableEntityRepository<Discipline>>();
            var disciplineTeacherRepositry = new Mock<IRepository<DisciplineTeacher>>();
            var schoolDisciplineRepository = new Mock<IRepository<SchoolDiscipline>>();

            var disciplineRepo = new List<Discipline>();

            var disciplineService = new DisciplineService(
                disciplineRepository.Object,
                schoolDisciplineRepository.Object,
                disciplineTeacherRepositry.Object,
                schoolRepository.Object);

            disciplineRepository.Setup(r => r.AddAsync(It.IsAny<Discipline>())).Callback((Discipline discipline) => disciplineRepo.Add(discipline));

            var inputMode = new CreateDisciplineInputModel
            {
                Name = "TestDiscipline",
            };

            // Act
            await disciplineService.CreateDisciplineAsync(inputMode);

            // Assert
            Assert.Equal("TestDiscipline", disciplineRepo[0].Name);
        }

        [Fact]
        public async Task CheckAddDisciplineToSchoolAsyncIsAddeDiscipline()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineRepository = new Mock<IDeletableEntityRepository<Discipline>>();
            var disciplineTeacherRepositry = new Mock<IRepository<DisciplineTeacher>>();
            var schoolDisciplineRepository = new Mock<IRepository<SchoolDiscipline>>();

            var schoolDisciplineRepo = new List<SchoolDiscipline>();

            var disciplineService = new DisciplineService(
                disciplineRepository.Object,
                schoolDisciplineRepository.Object,
                disciplineTeacherRepositry.Object,
                schoolRepository.Object);

            schoolDisciplineRepository.Setup(r => r.AddAsync(It.IsAny<SchoolDiscipline>()))
                .Callback((SchoolDiscipline schoolDisscipline) => schoolDisciplineRepo.Add(schoolDisscipline));

            // Act
            await disciplineService.AddDisciplineToSchoolAsync(20, 10);

            // Assert
            Assert.True(schoolDisciplineRepo.Count == 1);
            Assert.Equal(10, schoolDisciplineRepo.First().SchoolId);
        }

        [Fact]
        public async Task CheckRemoveDisciplineFromSchoolAsyncIsRemovedDiscipline()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineRepository = new Mock<IDeletableEntityRepository<Discipline>>();
            var disciplineTeacherRepository = new Mock<IRepository<DisciplineTeacher>>();
            var schoolDisciplineRepository = new Mock<IRepository<SchoolDiscipline>>();

            var schoolDisciplineRepo = new List<SchoolDiscipline>();
            var disciplineTeachersRepo = new List<DisciplineTeacher>();

            schoolDisciplineRepo.Add(new SchoolDiscipline
            {
                SchoolId = 2,
                DisciplineId = 1,
            });
            schoolDisciplineRepo.Add(new SchoolDiscipline
            {
                SchoolId = 3,
                DisciplineId = 1,
            });
            schoolDisciplineRepo.Add(new SchoolDiscipline
            {
                SchoolId = 2,
                DisciplineId = 3,
            });

            disciplineTeachersRepo.Add(new DisciplineTeacher
            {
                TeacherId = "test",
                DisciplineId = 1,
                Teacher = new Teacher
                { SchoolId = 2, Id = "test" },
            });
            disciplineTeachersRepo.Add(new DisciplineTeacher
            {
                TeacherId = "test1",
                DisciplineId = 1,
                Teacher = new Teacher
                { SchoolId = 3, Id = "test1" },
            });

            var disciplineService = new DisciplineService(
                disciplineRepository.Object,
                schoolDisciplineRepository.Object,
                disciplineTeacherRepository.Object,
                schoolRepository.Object);

            schoolDisciplineRepository.Setup(r => r.All()).Returns(schoolDisciplineRepo.AsQueryable());
            schoolDisciplineRepository.Setup(r => r.Delete(It.IsAny<SchoolDiscipline>()))
                .Callback((SchoolDiscipline schoolDiscipline) => schoolDisciplineRepo.Remove(schoolDiscipline));

            disciplineTeacherRepository.Setup(r => r.All()).Returns(disciplineTeachersRepo.AsQueryable());
            disciplineTeacherRepository.Setup(r => r.Delete(It.IsAny<DisciplineTeacher>()))
                .Callback((DisciplineTeacher disciplineTeacher) => disciplineTeachersRepo.Remove(disciplineTeacher));

            // Act
            await disciplineService.RemoveDisciplineFromSchoolAsync(1, 2);

            // Assert
            Assert.True(schoolDisciplineRepo.Count == 2);
            Assert.True(disciplineTeachersRepo.Count == 1);

        }

        [Fact]
        public void CheckGetNotStudiedDisciplinesReturnOnlyNotStudiedDisciplines()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var disciplineRepository = new Mock<IDeletableEntityRepository<Discipline>>();
            var disciplineTeacherRepositry = new Mock<IRepository<DisciplineTeacher>>();
            var schoolDisciplineRepository = new Mock<IRepository<SchoolDiscipline>>();

            var disciplineRepo = new List<Discipline>
            {
                new Discipline
                {
                    SchoolDisciplines = new List<SchoolDiscipline>
                         {
                         new SchoolDiscipline
                             {
                                 SchoolId = 1,
                             },
                         },
                },
                new Discipline
                {
                    SchoolDisciplines = new List<SchoolDiscipline>
                         {
                         new SchoolDiscipline
                             {
                                 SchoolId = 2,
                             },
                         },
                },
                new Discipline
                {
                    SchoolDisciplines = new List<SchoolDiscipline>
                         {
                         new SchoolDiscipline
                             {
                                 SchoolId = 3,
                             },
                         },
                },
            };

            var disciplineService = new DisciplineService(
                disciplineRepository.Object,
                schoolDisciplineRepository.Object,
                disciplineTeacherRepositry.Object,
                schoolRepository.Object);

            disciplineRepository.Setup(r => r.All()).Returns(disciplineRepo.AsQueryable());

            // Act
            var result = disciplineService.GetNotStudiedDisciplines(1);

            // Assert
            Assert.True(result.Count == 2);
        }
    }
}
