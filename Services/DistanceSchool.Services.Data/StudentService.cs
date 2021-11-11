namespace DistanceSchool.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;

    public class StudentService : IStudentService
    {
        private readonly IDeletableEntityRepository<Student> studentRepository;
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public StudentService(
            IDeletableEntityRepository<Student> studentRepository,
            IDeletableEntityRepository<Candidacy> candidacyRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.studentRepository = studentRepository;
            this.candidacyRepository = candidacyRepository;
            this.userRepository = userRepository;
        }

        public bool CheckUserIsStudent(string id)
        {
            return this.studentRepository.All().Any(x => x.ApplicationUserId == id);
        }

        public async Task<string> CreateNewStudentAsync(int id)
        {
            var candidacy = this.candidacyRepository.All().FirstOrDefault(x => x.Id == id);
            var student = new Student
            {
                FirstName = candidacy.FirstName,
                SecondName = candidacy.SecondName,
                LastName = candidacy.LastName,
                ApplicationUserId = candidacy.ApplicationUserId,
                ApplicationUser = this.candidacyRepository.All().Where(x => x.Id == id).Select(x => x.ApplicationUser).FirstOrDefault(),
                TeamId = (int)candidacy.TeamId,
            };


            await this.studentRepository.AddAsync(student);
            await this.studentRepository.SaveChangesAsync();
            await this.userRepository.SaveChangesAsync();

            return student.Id;
        }
    }
}
