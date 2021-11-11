namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Common;
    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using DistanceSchool.Web.ViewModels.Candidacyes;
    using DistanceSchool.Web.ViewModels.Managers;
    using DistanceSchool.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Http;

    public class CandidacyService : ICandidacyServices
    {
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;
        private readonly IDeletableEntityRepository<School> schoolRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IStudentService studentService;
        private readonly IDeletableEntityRepository<Student> studentRepository;
        private readonly IDeletableEntityRepository<Team> teamsRepository;

        public CandidacyService(
            IDeletableEntityRepository<Candidacy> candidacyRepository,
            IDeletableEntityRepository<School> schoolRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IFileHandlerService fileHandlerService,
            IStudentService studentService,
            IDeletableEntityRepository<Student> studentRepository,
            IDeletableEntityRepository<Team> teamsRepository)
        {
            this.candidacyRepository = candidacyRepository;
            this.schoolRepository = schoolRepository;
            this.userRepository = userRepository;
            this.fileHandlerService = fileHandlerService;
            this.studentService = studentService;
            this.studentRepository = studentRepository;
            this.teamsRepository = teamsRepository;
        }

        public async Task AddCandidacyAsync(CandidacyInputModel inputModel, CandidacyType candidacyType, string directoyPath)
        {
            if (candidacyType == CandidacyType.Manager)
            {
                var teacher = this.schoolRepository.All()
               .Where(x => x.Id == inputModel.SchoolId)
               .Select(x => x.Manager)
               .FirstOrDefault();

                if (teacher != null)
                {
                    return;
                }
            }

            var candidacy = new Candidacy
            {
                ApplicationUserId = inputModel.UserId,
                FirstName = inputModel.FirstName,
                SecondName = inputModel.SecondName,
                LastName = inputModel.LastName,
                BirthDate = inputModel.BirthDate,
                SchoolId = inputModel.SchoolId,
                Type = candidacyType,
            };

            var user = this.userRepository.All().FirstOrDefault(x => x.Id == inputModel.UserId);

            if (inputModel.ProfileImage != null)
            {
                var imageDirectoryPath = $"{directoyPath}/images/PrifileImage";
                var imageExtension = Path.GetExtension(inputModel.ProfileImage.FileName);
                user.ProfileImageExtension = imageExtension;

                await this.fileHandlerService.SaveFileAsync(inputModel.ProfileImage, imageDirectoryPath, inputModel.UserId + imageExtension);
            }

            if (inputModel.ApplicationDocuments != null)
            {
                var applicationDocumentsPath = $"{directoyPath}/applicationDocuments";

                var applicationDocumentExtension = Path.GetExtension(inputModel.ApplicationDocuments.FileName);
                user.ApplicationDocumentsExtension = applicationDocumentExtension;

                await this.fileHandlerService.SaveFileAsync(inputModel.ApplicationDocuments, applicationDocumentsPath, inputModel.UserId + applicationDocumentExtension);
            }

            await this.candidacyRepository.AddAsync(candidacy);
            await this.userRepository.SaveChangesAsync();
            await this.candidacyRepository.SaveChangesAsync();
        }

        public ICollection<CandidacyViewModel> GetAllManagerCandidacy()
        {
            return this.GetTeacherCandidacies(null, CandidacyType.Manager);
        }

        public async Task DeleteAllSchoolMangerCandidacyAsync(int schoolId)
        {
            var mangerCandidacy = this.candidacyRepository.All()
                .Where(x => x.SchoolId == schoolId & x.Type == CandidacyType.Manager)
                .ToList();

            foreach (var candidacy in mangerCandidacy)
            {
                candidacy.DeletedOn = DateTime.UtcNow;
                candidacy.IsDeleted = true;
            }

            await this.candidacyRepository.SaveChangesAsync();
        }

        public async Task DeleteCandicayAsync(int id)
        {
            var candidacyForDeleted = this.candidacyRepository.All().FirstOrDefault(x => x.Id == id);
            candidacyForDeleted.IsDeleted = true;
            candidacyForDeleted.DeletedOn = DateTime.UtcNow;

            await this.candidacyRepository.SaveChangesAsync();
        }

        public ICollection<CandidacyViewModel> GetSchoolCandidaciesAsync(int schoolId, CandidacyType candidacyType)
        {
            return this.GetTeacherCandidacies(schoolId, candidacyType);
        }

        public async Task AddStudentCandidacyAsync(StudentCandidacyInputModel inputModel, string directoyPath)
        {
            var candidacy = new Candidacy
            {
                ApplicationUserId = inputModel.UserId,
                FirstName = inputModel.FirstName,
                SecondName = inputModel.SecondName,
                LastName = inputModel.LastName,
                SchoolId = this.schoolRepository.All().FirstOrDefault(x => x.Teams.Any(y => y.Id == inputModel.TeamId)).Id,
                BirthDate = inputModel.BirthDate,
                TeamId = inputModel.TeamId,
                Type = CandidacyType.Student,
            };

            var user = this.userRepository.All().FirstOrDefault(x => x.Id == inputModel.UserId);

            if (inputModel.ProfileImage != null)
            {
                var imageDirectoryPath = $"{directoyPath}/images/PrifileImage";
                var imageExtension = Path.GetExtension(inputModel.ProfileImage.FileName);
                user.ProfileImageExtension = imageExtension;

                await this.fileHandlerService.SaveFileAsync(inputModel.ProfileImage, imageDirectoryPath, inputModel.UserId + imageExtension);
            }

            var applicationDocumentsPath = $"{directoyPath}/studentApplicationDocuments";

            var applicationDocumentExtension = Path.GetExtension(inputModel.ApplicationDocuments.FileName);
            user.ApplicationDocumentsExtension = applicationDocumentExtension;

            await this.fileHandlerService.SaveFileAsync(inputModel.ApplicationDocuments, applicationDocumentsPath, inputModel.UserId + applicationDocumentExtension);

            await this.candidacyRepository.AddAsync(candidacy);
            await this.userRepository.SaveChangesAsync();
            await this.candidacyRepository.SaveChangesAsync();
        }

        public async Task<bool> AddAllreadyStudentCandidacyAsync(StudentCandidacyInputModel inputModel)
        {
            var isUserStudent = this.studentService.CheckUserIsStudent(inputModel.UserId);
            if (isUserStudent)
            {
                var candidacy = this.studentRepository.All().Where(x => x.ApplicationUserId == inputModel.UserId)
                     .Select(x => new Candidacy
                     {
                         TeamId = inputModel.TeamId,
                         ApplicationUserId = inputModel.UserId,
                         FirstName = x.FirstName,
                         SecondName = x.SecondName,
                         LastName = x.LastName,
                         SchoolId = this.schoolRepository.All().FirstOrDefault(x => x.Teams.Any(y => y.Id == inputModel.TeamId)).Id,
                         BirthDate = x.BirthDate,
                     }).FirstOrDefault();

                await this.candidacyRepository.AddAsync(candidacy);
                await this.candidacyRepository.SaveChangesAsync();
            }

            return isUserStudent;
        }

        public StudentCandidacyViewModel GetAllStudetnCandidacies(int schoolId, int page)
        {
            if (page == 0)
            {
                page++;
            }

            var viewModel = this.schoolRepository.All()
                .Where(x => x.Id == schoolId)
                .Select(x => new StudentCandidacyViewModel
                {
                    SchoolId = x.Id,
                    SchoolName = x.Name,
                    SchoolManager = x.Manager.Teacher.FirstName + " " + x.Manager.Teacher.LastName,
                    Teams = this.GetAllTeamsCandidacies(schoolId, page),
                    CurentPage = page,
                    LastPage = (int)Math.Ceiling(x.Teams.Count / 4.0),
                }).FirstOrDefault();

            return viewModel;
        }

        private ICollection<CandidacyViewModel> GetTeacherCandidacies(int? schoolId, CandidacyType candidacyType)
        {
            var result = this.candidacyRepository
            .All()
            .Where(x => x.FirstName != null && x.Type == candidacyType && (x.SchoolId == schoolId || schoolId == null))
            .Select(x => new CandidacyViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                SecondName = x.SecondName,
                LastName = x.LastName,
                SchoolName = x.School.Name,
                Year = DateTime.UtcNow.Year - ((DateTime)x.BirthDate).Year,
                ApplicationDocumenstUrl = $"/applicationDocuments/{x.ApplicationUserId}{x.ApplicationUser.ApplicationDocumentsExtension}",
                ProfilPictutreUrl = x.ApplicationUser.ProfileImageExtension == null ? null 
                    : $"/Images/PrifileImage/{x.ApplicationUserId}{x.ApplicationUser.ProfileImageExtension}",
            }).ToList();

            var teacherCandidacy = this.candidacyRepository.All()
                .Where(x => x.FirstName == null && x.Type == candidacyType && (x.SchoolId == schoolId || schoolId == null))
                .Select(x => new CandidacyViewModel
                {
                    Id = x.Id,
                    FirstName = x.ApplicationUser.Teacher.FirstName,
                    SecondName = x.ApplicationUser.Teacher.SecondName,
                    LastName = x.ApplicationUser.Teacher.LastName,
                    SchoolName = x.School.Name,
                    Year = DateTime.UtcNow.Year - x.ApplicationUser.Teacher.BirthDate.Year,
                    ApplicationDocumenstUrl = $"/applicationDocuments/{x.ApplicationUserId}{x.ApplicationUser.ApplicationDocumentsExtension}",
                    ProfilPictutreUrl = x.ApplicationUser.ProfileImageExtension == null ? null
                        : $"/Images/PrifileImage/{x.ApplicationUserId}{x.ApplicationUser.ProfileImageExtension}",
                }).ToList();

            result.AddRange(teacherCandidacy);
            var orderedReuslt = result.OrderBy(x => x.SchoolName).ThenBy(x => x.FirstName).ToList();

            return result;
        }

        private ICollection<TeamsCandidacyViewModel> GetAllTeamsCandidacies(int schoolId, int page)
        {
            var viewModel = this.teamsRepository.All()
                .Where(x => x.SchoolId == schoolId)
                .Select(t => new TeamsCandidacyViewModel
                {
                    Id = t.Id,
                    TeamName = t.Name + " " + t.Level,
                    Candidacies = this.candidacyRepository
                        .All()
                        .Where(x => x.TeamId == t.Id)
                         .Select(x => new CandidacyViewModel
                         {
                             Id = x.Id,
                             FirstName = x.FirstName,
                             SecondName = x.SecondName,
                             LastName = x.LastName,
                             Year = DateTime.UtcNow.Year - ((DateTime)x.BirthDate).Year,
                             ApplicationDocumenstUrl = $"/studentApplicationDocuments/{x.ApplicationUserId}{x.ApplicationUser.ApplicationDocumentsExtension}",
                             ProfilPictutreUrl = x.ApplicationUser.ProfileImageExtension == null ? null : $"/Images/PrifileImage/{x.ApplicationUserId}{x.ApplicationUser.ProfileImageExtension}",
                         }).ToList(),
                })
                .OrderByDescending(y => y.Candidacies.Count)
                .ToList();

            var result = new List<TeamsCandidacyViewModel>();
            var curentLastIndex = page * 4;
            var lastIndex = viewModel.Count;
            var endPage = curentLastIndex < lastIndex ? curentLastIndex : lastIndex;

            for (int i = (page - 1) * 4; i < endPage; i++)
            {
                result.Add(viewModel[i]);
            }

            return result;
        }
    }
}
