namespace DistanceSchool.Services
{
    public interface ICheckRoleAttributeService
    {
        bool CheckUserIsManager(string userId);

        bool CheckUserIsManager(string userId, int schoolId);

        bool CheckUserIsManagerToCandidacy(string userId, int candidacylId);

        bool CheckUserIsManagerWithTeacherId(string userId, string teacherId);

        bool CheckUserIsManagerWithTeamId(string userId, int temId);

        bool CheckIsUser(string userId, string teacherId);

        bool CheckUserIsManagerWithTeacherTeamId(string userId, int teacherTheamId);

        bool CheckUserIsUserWithTeacherTeamId(string userId, int teacherTheamId);

        bool CheckHasAccessToTeam(string userId, int teamId);

    }
}
