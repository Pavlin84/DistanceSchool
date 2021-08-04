namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Candidacyes;
    using DistanceSchool.Web.ViewModels.Managers;
    using DistanceSchool.Web.ViewModels.Teams;

    public interface ICandidacyServices
    {
        Task AddCandidacyAsync(CandidacyInputModel inputModel, CandidacyType candidacyType, string directoyPath);

        ICollection<CandidacyViewModel> GetAllManagerCandidacy();

        Task DeleteCandicayAsync(int id);

        Task DeleteAllSchoolMangerCandidacyAsync(int schoolId);

        ICollection<CandidacyViewModel> GetSchoolCandidaciesAsync(int schoolId, CandidacyType candidacyType);

        Task AddStudentCandidacyAsync(StudentCandidacyInputModel inputModel, string directoyPath);

        Task<bool> AddAllreadyStudentCandidacyAsync(StudentCandidacyInputModel inputModel);

        StudentCandidacyViewModel GetAllStudetnCandidacies(int schoolId);
    }
}
