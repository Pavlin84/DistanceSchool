namespace DistanceSchool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using Moq;
    using Xunit;

    public class StudentServiceTest
    {
        [Fact]
        public async Task CheckCreateNewStudentAsyncIsCreatedStudent()
        {
            // Arrange
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();

            var studentRepo = new List<Student>();
            var candidacyRepo = new List<Candidacy>()
            {
                new Candidacy
                {
                    Id = 1,
                    ApplicationUserId = "isTrue",
                    ApplicationUser = new ApplicationUser(),
                    TeamId = 1,
                },
                new Candidacy
                {
                    Id = 2,
                    ApplicationUserId = "isFalse",
                    ApplicationUser = new ApplicationUser(),
                    TeamId = 1,
                },
            };

            studentRepository.Setup(r => r.AddAsync(It.IsAny<Student>())).Callback((Student student) => studentRepo.Add(student));
            candidacyRepository.Setup(r => r.All()).Returns(candidacyRepo.AsQueryable());

            var studentService = new StudentService(studentRepository.Object, candidacyRepository.Object, userRepository.Object);

            // Act
            await studentService.CreateNewStudentAsync(1);

            // Assert
            Assert.True(studentRepo.Count == 1);
            Assert.Equal("isTrue", studentRepo[0].ApplicationUserId);

        }
    }
}
