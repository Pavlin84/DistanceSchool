namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Managers;
    using DistanceSchool.Web.ViewModels.Schools;
    using DistanceSchool.Web.ViewModels.Teachers;

    public interface ISchoolService
    {
        Task AddSchoolAsync(AddSchoolInputModel inputModel);

        List<AllScholsViewModel> AllSchool();

        string GetSchoolName(int id);

        Task<int> AddManagerAsync(int candidacyId);

        Task CheckAndDeleteOldSchoolManager(string userId);

        OneSchoolViewModel GetSchoolData(int schoolId);

        bool IsUserMangerToSchool(string userId, int schoolId);

        Task RemoveManagerAsync(int schoolId);

        int GetSchoolIdWithManager(string managerId);

        SchoolManagerHomeViewModel GetManagerHomePageData(string managerId);

        bool IsUserManger(string userId);

        Task<string> AddTeacherToSchool(int candidacyId);
    }
}
