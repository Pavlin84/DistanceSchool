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

        Task<AddTeacherDtoModel> CreateTeacherAsync(int candidacyId);

        Task ChangeSchoolIdAsync(string id, int schoolId);

        ICollection<TeacherForOneSchoolViewModel> GetAllTeacherFromSchool(int schoolId);
    }
}
