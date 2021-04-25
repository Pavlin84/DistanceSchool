namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Teachers;

    public class TeacherService : ITeacherServisce
    {
        private readonly IDeletableEntityRepository<Teacher> teacherRepository;
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;

        public TeacherService(IDeletableEntityRepository<Teacher> teacherRepository, IDeletableEntityRepository<Candidacy> candidacyRepository)
        {
            this.teacherRepository = teacherRepository;
            this.candidacyRepository = candidacyRepository;
        }

        public bool IsTeacher(string userId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == userId);

            return teacher != null;
        }

        public async Task<AddMangerDtoModel> CreateTeacherAsync(int candidacyId)
        {
            var teacherDto = this.candidacyRepository.All()
              .Where(x => x.Id == candidacyId)
              .Select(x => new AddMangerDtoModel
              {
                  ManagerId = x.ApplicationUser.Id,
                  SchoolId = (int)x.SchoolId,
                  TeacherId = x.ApplicationUser.TeacherId,
              }).FirstOrDefault();

            if (teacherDto.TeacherId == null)
            {
                var candidacyTeacher = this.candidacyRepository.All().FirstOrDefault(x => x.Id == candidacyId);

                var teacher = new Teacher
                {
                    FirstName = candidacyTeacher.FirstName,
                    SecondName = candidacyTeacher.SecondName,
                    LastName = candidacyTeacher.LastName,
                    BirthDate = (DateTime)candidacyTeacher.BirthDate,
                    ApplicationUserId = candidacyTeacher.ApplicationUserId,
                    ApplicationUser = this.candidacyRepository.All().Where(x => x.Id == candidacyId).Select(x => x.ApplicationUser).FirstOrDefault(),
                    SchoolId = (int)candidacyTeacher.SchoolId,
                };

                teacherDto = new AddMangerDtoModel
                {
                    ManagerId = teacher.ApplicationUserId,
                    SchoolId = teacher.SchoolId,
                };

                await this.teacherRepository.AddAsync(teacher);
                await this.teacherRepository.SaveChangesAsync();
            }

            return teacherDto;
        }

        public async Task CahngeSchoolIdAsync(string id, int schoolId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == id);
            teacher.SchoolId = schoolId;

            await this.teacherRepository.SaveChangesAsync();
        }
    }
}
