namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teachers;

    public interface ITeacherService
    {
        bool IsTeacher(string userId);

        Task<AddTeacherDtoModel> CreateTeacherAsync(int candidacyId);

        Task<string> ChangeSchoolIdAsync(string id, int schoolId);

        ICollection<TeacherForOneSchoolViewModel> GetAllTeacherFromSchool(int schoolId);

        bool IsUserTeacherToSchool(string userId, int schoolId);

        int GetSchoolId(string userId);

        OneTeacherViewModel GetTeacherData(string id, string userId);

        DisciplineHandlerViewModel GetTeacherDisciplines(string teacherId);

        Task AddDisciplinesToTeacherAsync(AddDisciplineToTeacherInputModel inputModel);

        string GetTeacherId(string userId);

        ShiftsTecherViewModel ShiftsTeacher(int teacherTeamId);
    }
}
