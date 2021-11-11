namespace DistanceSchool.Services
{
    using System.Linq;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;

    public class CheckRoleAttributeService : ICheckRoleAttributeService
    {
        private readonly IDeletableEntityRepository<School> schoolRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<TeacherTeam> teacherTeamRepository;

        public CheckRoleAttributeService(
            IDeletableEntityRepository<School> schoolRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<TeacherTeam> teacherTeamRepository)
        {
            this.schoolRepository = schoolRepository;
            this.userRepository = userRepository;
            this.teacherTeamRepository = teacherTeamRepository;
        }

        public bool CheckIsUser(string userId, string teacherId)
        {
            var curentUser = this.userRepository.All().FirstOrDefault(x => x.Id == userId).TeacherId;

            return curentUser == teacherId;
        }

        public bool CheckUserIsManager(string userId)
        {
            return this.schoolRepository.All().Any(x => x.ManagerId == userId);
        }

        public bool CheckUserIsManager(string userId, int schoolId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == schoolId && x.ManagerId == userId);

            return school != null;
        }

        public bool CheckUserIsManagerToCandidacy(string userId, int candidacylId)
        {
            return this.schoolRepository.All().Any(x => x.Candidacies.Any(y => y.Id == candidacylId) && x.ManagerId == userId);
        }

        public bool CheckUserIsManagerWithTeacherId(string userId, string teacherId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Teachers.Any(y => y.Id == teacherId));
            return this.CheckUserIsManager(userId, school.Id);
        }

        public bool CheckUserIsManagerWithTeamId(string userId, int temId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Teams.Any(x => x.Id == temId));
            return this.CheckUserIsManager(userId, school.Id);
        }

        public bool CheckUserIsManagerWithTeacherTeamId(string userId, int teacherTheamId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Teams.Any(y => y.TeacherTeams.Any(t => t.Id == teacherTheamId)));
            return this.CheckUserIsManager(userId, school.Id);
        }

        public bool CheckUserIsUserWithTeacherTeamId(string userId, int teacherTheamId)
        {
            var teacher = this.teacherTeamRepository.All().FirstOrDefault(x => x.Id == teacherTheamId);
            return this.CheckIsUser(userId, teacher.TeacherId);
        }
    }
}
