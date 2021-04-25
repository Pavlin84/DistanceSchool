namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Common;
    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Administration.Dashboard;
    using DistanceSchool.Web.ViewModels.Candidacyes;

    public class CandidacyServices : ICandidacyServices
    {
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;
        private readonly IDeletableEntityRepository<School> schoolRepository;

        public CandidacyServices(IDeletableEntityRepository<Candidacy> candidacyRepository, IDeletableEntityRepository<School> schoolRepository)
        {
            this.candidacyRepository = candidacyRepository;
            this.schoolRepository = schoolRepository;
        }

        public async Task AddCandidacyAsync(ManagerCandidacyInputModel inputModel)
        {
            var teacher = this.schoolRepository.All()
                .Where(x => x.Id == inputModel.SchoolId)
                .Select(x => x.Manager)
                .FirstOrDefault();

            if (teacher != null)
            {
                return;
            }

            var candidacy = new Candidacy
            {
                ApplicationUserId = inputModel.UserId,
                FirstName = inputModel.FirstName,
                SecondName = inputModel.SecondName,
                LastName = inputModel.LastName,
                BirthDate = inputModel.BirthDate,
                SchoolId = inputModel.SchoolId,
                ApplicationDocumentsPath = GlobalConstants.TeacherApplicationDocumentsPath + inputModel.UserId,
                Type = CandidacyType.Manager,
            };

            await this.candidacyRepository.AddAsync(candidacy);
            await this.candidacyRepository.SaveChangesAsync();
        }

        public ICollection<MangerCandidacyViewModel> GetAllManagerCandidacy()
        {
            var result = this.candidacyRepository
                .All()
                .Where(x => x.FirstName != null && x.Type == CandidacyType.Manager)
                .Select(x => new MangerCandidacyViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    LastName = x.LastName,
                    SchoolName = x.School.Name,
                    Year = DateTime.UtcNow.Year - ((DateTime)x.BirthDate).Year,
                }).ToList();

            var teacherCandidacy = this.candidacyRepository.All()
                .Where(x => x.FirstName == null && x.Type == CandidacyType.Manager)
                .Select(x => new MangerCandidacyViewModel
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

            return orderedReuslt;
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
            var candidacyForDeleted = this.candidacyRepository.All().FirstOrDefault(x => x.Id == id && x.Type == CandidacyType.Manager);
            candidacyForDeleted.IsDeleted = true;
            candidacyForDeleted.DeletedOn = DateTime.UtcNow;

            await this.candidacyRepository.SaveChangesAsync();
        }
    }
}
