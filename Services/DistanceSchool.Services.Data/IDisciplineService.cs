﻿namespace DistanceSchool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Disciplines;

    public interface IDisciplineService
    {
        Task CreateDisciplineAsync(CreateDisciplineInputModel input);

        ICollection<string> GetAllDiscplineName();

        AllDisciplinesViewModel GetAllDisciplines();

        bool IsExsist(string discipline);

        ICollection<DisciplineForOneSchoolViewModel> GetNotStudiedDisciplines(int schoolId);

        Task AddDisciplineToSchoolAsync(int disciplineId, int schoolId);

        Task RemoveDisciplineFromSchoolAsync(int disciplineId, int schoolId);

        Dictionary<string, int> GetSchoolDisciplines(int schoolId);

        ICollection<DisciplineForOneTeamViewModel> GetTeamDisciplines(int id);

        DisciplineHandlerViewModel GetAllDisciplineForTeam(int id);
    }
}
