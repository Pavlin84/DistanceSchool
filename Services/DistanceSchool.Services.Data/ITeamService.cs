namespace DistanceSchool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teams;

    public interface ITeamService
    {
        Task<int> AddTeam(AddTeamInputModel team);

        OneTeamViewModel GetTeamData(int id, string userId);

        Task AddDiscipineToTeamAsync(AddDisciplinesToTeamInputModel inputModel);

        Task<int> AddStudentToTeamAsync(int id);

        Task ChangeTeacher(int teacherTeamId, string teacherId);

        TeamPassportVewModel GetTeamPassportData(int teacherTeamId);

        List<MatchedTeamsViewModel> GetMatchedTeams(List<int> disciplinesId);

        List<MatchedTeamsViewModel> GetAllMatchedTeams(List<int> disciplinesId);

    }

}
