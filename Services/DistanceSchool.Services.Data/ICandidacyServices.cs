namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Candidacyes;

    public interface ICandidacyServices
    {
        Task AddCandidacyAsync(ManagerCandidacyInputModel inputModel);

        ICollection<MangerCandidacyViewModel> GetAllManagerCandidacy();

        Task DeleteCandicayAsync(int id);

        Task DeleteAllSchoolMangerCandidacyAsync(int schoolId);
    }
}
