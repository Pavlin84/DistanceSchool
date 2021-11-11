namespace DistanceSchool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Candidacyes;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class CandidacyServicesTest
    {
        [Fact]
        public async Task CheckDeleteAllSchoolMangerCandidacyAsyncIsDeleteCandidacy()
        {
            // Arrange
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var fileHandlerService = new Mock<IFileHandlerService>();
            var studentService = new Mock<IStudentService>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var teamsRepository = new Mock<IDeletableEntityRepository<Team>>();

            var listRepos = new List<Candidacy>
            {
                new Candidacy
                { Id = 1, IsDeleted = false },
                new Candidacy
                { Id = 2, IsDeleted = false },
                new Candidacy
                { Id = 3, IsDeleted = false },
                new Candidacy
                { Id = 4, IsDeleted = false },
            };

            candidacyRepository.Setup(r => r.All()).Returns(listRepos.AsQueryable());

            var candidacyService = new CandidacyService(
                candidacyRepository.Object,
                schoolRepository.Object,
                userRepository.Object,
                fileHandlerService.Object,
                studentService.Object,
                studentRepository.Object,
                teamsRepository.Object);

            // Act
            await candidacyService.DeleteCandicayAsync(1);

            // Assert
            Assert.True(listRepos[0].IsDeleted);
            for (int i = 1; i < listRepos.Count; i++)
            {
                Assert.True(!listRepos[i].IsDeleted);
            }
        }

        [Fact]
        public async Task CheckAddCandidacyAsyncMethodIsAddedCandidacy()
        {
            // Arrange
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var fileHandlerService = new Mock<IFileHandlerService>();
            var studentService = new Mock<IStudentService>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var teamsRepository = new Mock<IDeletableEntityRepository<Team>>();

            var listRepos = new List<Candidacy>();

            candidacyRepository.Setup(r => r.All()).Returns(listRepos.AsQueryable());
            candidacyRepository.Setup(r => r.AddAsync(It.IsAny<Candidacy>())).Callback((Candidacy candidacy) => listRepos.Add(candidacy));

            var candidacyService = new CandidacyService(
                candidacyRepository.Object,
                schoolRepository.Object,
                userRepository.Object,
                fileHandlerService.Object,
                studentService.Object,
                studentRepository.Object,
                teamsRepository.Object);

            var inputModel = new TeacherCandidacyInputModel
            {
                SchoolId = 1,
                UserId = "2",
            };

            // Act
            await candidacyService.AddCandidacyAsync(inputModel, CandidacyType.Teacher, " ");
            await candidacyService.AddCandidacyAsync(inputModel, CandidacyType.Teacher, " ");

            // Assert
            Assert.True(listRepos[0].ApplicationUserId == "2" && listRepos[1].SchoolId == 1);
        }

        [Fact]
        public async Task CheckAddAllreadyStudentCandidacyAsyncIsAddedCandidacy()
        {
            // Arrange
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var fileHandlerService = new Mock<IFileHandlerService>();
            var studentService = new Mock<IStudentService>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var teamsRepository = new Mock<IDeletableEntityRepository<Team>>();

            var candidacyRepos = new List<Candidacy>
            {
            };
            var schoolRepos = new List<School>
            {
                new School
                {
                    Id = 1,
                    Teams = new List<Team>
                    { new Team { Id = 2 } },
                },
                new School(),
            };

            var studentRepo = new List<Student>
            {
                new Student
                {
                    ApplicationUserId = "test",
                    FirstName = "First",
                    SecondName = "Second",
                    LastName = "Last",
                },
                new Student
                {
                    ApplicationUserId = "NotValid",
                },
            };
            candidacyRepository.Setup(r => r.All()).Returns(candidacyRepos.AsQueryable());
            candidacyRepository.Setup(r => r.AddAsync(It.IsAny<Candidacy>())).Callback((Candidacy candidacy) => candidacyRepos.Add(candidacy));
            schoolRepository.Setup(r => r.All()).Returns(schoolRepos.AsQueryable());
            studentService.Setup(s => s.CheckUserIsStudent(It.IsAny<string>())).Returns(true);
            studentRepository.Setup(r => r.All()).Returns(studentRepo.AsQueryable());

            var candidacyService = new CandidacyService(
                candidacyRepository.Object,
                schoolRepository.Object,
                userRepository.Object,
                fileHandlerService.Object,
                studentService.Object,
                studentRepository.Object,
                teamsRepository.Object);

            var inputModel = new StudentCandidacyInputModel
            {
                TeamId = 2,
                UserId = "test",
            };

            // Act
            await candidacyService.AddAllreadyStudentCandidacyAsync(inputModel);

            // Assert
            Assert.True(candidacyRepos.Count == 1);
            Assert.Equal("test", candidacyRepos[0].ApplicationUserId);
            Assert.Equal("First", candidacyRepos[0].FirstName);
        }

        [Fact]
        public async Task CheckAddStudentCandidacyAsyncMethodIsAddedCandidacy()
        {
            // Arrange
            var candidacyRepository = new Mock<IDeletableEntityRepository<Candidacy>>();
            var schoolRepository = new Mock<IDeletableEntityRepository<School>>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var fileHandlerService = new Mock<IFileHandlerService>();
            var studentService = new Mock<IStudentService>();
            var studentRepository = new Mock<IDeletableEntityRepository<Student>>();
            var teamsRepository = new Mock<IDeletableEntityRepository<Team>>();
            var fakeFormFile = new Mock<IFormFile>();
            var listRepos = new List<Candidacy>();

            var schoolRepos = new List<School>
            {
                new School
                {
                    Id = 1,
                    Teams = new List<Team>
                    { new Team { Id = 2 } },
                },
                new School(),
            };
            var userRepo = new List<ApplicationUser>
            {
               new ApplicationUser
               {
                   Id = "2",
               },
            };

            candidacyRepository.Setup(r => r.AddAsync(It.IsAny<Candidacy>())).Callback((Candidacy candidacy) => listRepos.Add(candidacy));
            schoolRepository.Setup(r => r.All()).Returns(schoolRepos.AsQueryable());
            fakeFormFile.Setup(f => f.FileName).Returns("file.test");
            userRepository.Setup(r => r.All()).Returns(userRepo.AsQueryable());

            var candidacyService = new CandidacyService(
                candidacyRepository.Object,
                schoolRepository.Object,
                userRepository.Object,
                fileHandlerService.Object,
                studentService.Object,
                studentRepository.Object,
                teamsRepository.Object);

            var inputModel = new StudentCandidacyInputModel
            {
                TeamId = 2,
                UserId = "2",
                FirstName = "First",
                SecondName = "Second",
                LastName = "Last",
                ApplicationDocuments = fakeFormFile.Object,
            };

            // Act
            await candidacyService.AddStudentCandidacyAsync(inputModel,"");

            // Assert
            Assert.True(listRepos[0].ApplicationUserId == "2" && listRepos[0].SchoolId == 1 && listRepos[0].FirstName == "First");
        }
    }
}
