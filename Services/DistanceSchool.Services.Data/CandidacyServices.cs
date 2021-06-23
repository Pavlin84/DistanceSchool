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
    using Microsoft.AspNetCore.Http;

    public class CandidacyServices : ICandidacyServices
    {
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;
        private readonly IDeletableEntityRepository<School> schoolRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IFileHandlerService fileHandlerService;

        public CandidacyServices(
            IDeletableEntityRepository<Candidacy> candidacyRepository,
            IDeletableEntityRepository<School> schoolRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IFileHandlerService fileHandlerService)
        {
            this.candidacyRepository = candidacyRepository;
            this.schoolRepository = schoolRepository;
            this.userRepository = userRepository;
            this.fileHandlerService = fileHandlerService;
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

                await this.fileHandlerService.SaveFile(inputModel.ProfileImage, imageDirectoryPath, inputModel.UserId + imageExtension);
            }

            var applicationDocumentsPath = $"{directoyPath}/applicationDocuments";

            var applicationDocumentExtension = Path.GetExtension(inputModel.ApplicationDocuments.FileName);
            user.ApplicationDocumentsExtension = applicationDocumentExtension;

            await this.fileHandlerService.SaveFile(inputModel.ApplicationDocuments, applicationDocumentsPath, inputModel.UserId + applicationDocumentExtension);

            await this.candidacyRepository.AddAsync(candidacy);
            await this.userRepository.SaveChangesAsync();
            await this.candidacyRepository.SaveChangesAsync();
        }

        private async Task SaveFile(IFormFile sourseFile, string directoyPath, string fileName)
        {
            Directory.CreateDirectory(directoyPath);

            using Stream imageStream = new FileStream($"{directoyPath}/{fileName}", FileMode.Create);
            await sourseFile.CopyToAsync(imageStream);
        }

        public ICollection<CandidacyViewModel> GetAllManagerCandidacy()
        {
            return this.GetCandidacies(null, CandidacyType.Manager);
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

        public ICollection<CandidacyViewModel> GetSchoolCandidacies(int schoolId, CandidacyType candidacyType)
        {

            return this.GetCandidacies(schoolId, candidacyType);
        }

        private ICollection<CandidacyViewModel> GetCandidacies(int? schoolId, CandidacyType candidacyType)
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
                }).ToList();

            result.AddRange(teacherCandidacy);
            var orderedReuslt = result.OrderBy(x => x.SchoolName).ThenBy(x => x.FirstName).ToList();

            return result;
        }
    }
}
