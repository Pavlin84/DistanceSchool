namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Teachers;

    public interface ITeacherServisce
    {
        bool IsTeacher(string userId);

        Task<AddMangerDtoModel> CreateTeacherAsync(int candidacyId);

        Task CahngeSchoolIdAsync(string id, int schoolId);
    }
}
