namespace DistanceSchool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Schools;
    using Moq;
    using Xunit;

    public class SchoolServiceTest
    {
        [Fact]
        public async Task CheckAddSchoolAsyncIsCeatedNewSchool()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var teacherService = new Mock<ITeacherService>();
            var candidacyService = new Mock<ICandidacyServices>();
            var disciplineService = new Mock<IDisciplineService>();

            var schoolRepo = new List<School>();

            var schoolService = new SchoolService(
                schoolRepository.Object,
                teacherService.Object,
                candidacyService.Object,
                disciplineService.Object);

            schoolRepository.Setup(r => r.AddAsync(It.IsAny<School>())).Callback((School school) => schoolRepo.Add(school));

            var inputModel = new AddSchoolInputModel
            {
                Name = "test",
            };

            // Act
            await schoolService.AddSchoolAsync(inputModel);

            // Assert
            Assert.Equal("test", schoolRepo[0].Name);
            Assert.True(schoolRepo.Count == 1);
        }

        [Fact]
        public void CheckAllSchoolIsReturnAllSchool()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var teacherService = new Mock<ITeacherService>();
            var candidacyService = new Mock<ICandidacyServices>();
            var disciplineService = new Mock<IDisciplineService>();

            var schoolRepo = new List<School>
            {
                new School
                {
                    Id = 1,
                    Manager = new ApplicationUser
                    {
                        Teacher = new Teacher
                        {
                            FirstName = " ",
                            LastName = " ",
                        },
                    },
                },
                new School
                {
                    Id = 2,
                    Manager = new ApplicationUser
                    {
                        Teacher = new Teacher
                        {
                            FirstName = " ",
                            LastName = " ",
                        },
                    },
                },
                new School
                {
                    Id = 3,
                    Manager = new ApplicationUser
                    {
                        Teacher = new Teacher
                        {
                            FirstName = " ",
                            LastName = " ",
                        },
                    },
                },
            };

            var schoolService = new SchoolService(
                schoolRepository.Object,
                teacherService.Object,
                candidacyService.Object,
                disciplineService.Object);

            schoolRepository.Setup(r => r.All()).Returns(schoolRepo.AsQueryable());

            // Act
            var viewModel = schoolService.AllSchool();

            // Assert
            Assert.True(viewModel.Count == 3);

            for (int i = 0; i < viewModel.Count; i++)
            {
                var expectedId = i + 1;

                Assert.Equal(expectedId.ToString(), viewModel[i].Id);
            }
        }

        [Fact]
        public void CheckGetSchoolIdWithManagerIsReturnCorrectSchool()
        {
            // Arrange
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var teacherService = new Mock<ITeacherService>();
            var candidacyService = new Mock<ICandidacyServices>();
            var disciplineService = new Mock<IDisciplineService>();

            var schoolRepo = new List<School>
            {
                new School
                {
                    Id = 1,
                    ManagerId = "a",
                },
                new School
                {
                    Id = 2,
                    ManagerId = "b",
                },
                new School
                {
                    Id = 3,
                    ManagerId = "c",
                },
            };

            var schoolService = new SchoolService(
                schoolRepository.Object,
                teacherService.Object,
                candidacyService.Object,
                disciplineService.Object);

            schoolRepository.Setup(r => r.All()).Returns(schoolRepo.AsQueryable());

            // Act
            var schoolId = schoolService.GetSchoolIdWithManager("a");

            // Assert
            Assert.Equal(1, schoolId);
        }
    }
}
