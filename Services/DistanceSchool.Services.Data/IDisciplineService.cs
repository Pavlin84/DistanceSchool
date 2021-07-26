namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teams;

    public interface IDisciplineService
    {
        Task CreateDisciplineAsync(CreateDisciplineInputModel input);

        ICollection<string> GetAllDiscpline();

        bool IsExsist(string discipline);

        ICollection<DisciplineForOneSchoolViewModel> GetNotStudiedDisciplines(int schoolId);

        Task AddDisciplineToSchoolAsync(int disciplineId, int schoolId);

        Task RemoveDisciplineFromSchoolAsync(int disciplineId, int schoolId);

        Dictionary<string, int> GetSchoolDisciplines(int schoolId);

        ICollection<DisciplineForOneTeamViewModel> GetTeamDisciplines(int id);

        AddDisciplineToTeamViewModel GetAllDisciplineForTeam(int id);
    }
}
