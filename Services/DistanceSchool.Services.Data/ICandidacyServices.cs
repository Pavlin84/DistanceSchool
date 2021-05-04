namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Candidacyes;

    public interface ICandidacyServices
    {
        Task AddCandidacyAsync(CandidacyInputModel inputModel, CandidacyType candidacyType);

        ICollection<MangerCandidacyViewModel> GetAllManagerCandidacy();

        Task DeleteCandicayAsync(int id);

        Task DeleteAllSchoolMangerCandidacyAsync(int schoolId);
    }
}
