namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Schools;

    public interface ISchoolService
    {
        Task AddSchoolAsync(AddSchoolInputModel inputModel);

        List<AllScholsViewModel> AllSchool();

        string GetSchoolName(int id);

        Task AddManagerAsync(int candidacyId);

        OneSchoolViewModel GetSchoolData(int schoolId);

        bool IsUserManger(string userId, int schoolId);

        Task RemoveManagerAsync(int schoolId);
    }
}
