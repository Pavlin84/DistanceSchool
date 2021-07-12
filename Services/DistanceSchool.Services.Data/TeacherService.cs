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

    public class TeacherService : ITeacherService
    {
        private readonly IDeletableEntityRepository<Teacher> teacherRepository;
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;

        public TeacherService(
            IDeletableEntityRepository<Teacher> teacherRepository,
            IDeletableEntityRepository<Candidacy> candidacyRepository)
        {
            this.teacherRepository = teacherRepository;
            this.candidacyRepository = candidacyRepository;
        }

        public bool IsTeacher(string userId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == userId);

            return teacher != null;
        }

        public async Task<AddTeacherDtoModel> CreateTeacherAsync(int candidacyId)
        {
            var teacherDto = this.candidacyRepository.All()
              .Where(x => x.Id == candidacyId)
              .Select(x => new AddTeacherDtoModel
              {
                  UserId = x.ApplicationUser.Id,
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

                teacherDto = new AddTeacherDtoModel
                {
                    UserId = teacher.ApplicationUserId,
                    SchoolId = teacher.SchoolId,
                };

                await this.teacherRepository.AddAsync(teacher);
                await this.teacherRepository.SaveChangesAsync();
            }

            return teacherDto;
        }

        public async Task ChangeSchoolIdAsync(string id, int schoolId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == id);
            teacher.SchoolId = schoolId;

            await this.teacherRepository.SaveChangesAsync();
        }

        public ICollection<TeacherForOneSchoolViewModel> GetAllTeacherFromSchool(int schoolId)
        {
            var viewModel = this.teacherRepository.All()
                .Where(x => x.SchoolId == schoolId)
                .OrderByDescending(x => x.BirthDate)
                .Select(x => new TeacherForOneSchoolViewModel
                {
                    Id = x.Id,
                    UserId = x.ApplicationUserId,
                    Name = x.FirstName + " " + x.LastName,
                    Disciplines = x.DisciplineTeachers.Select(x => x.Discipline.Name).ToList(),
                }).ToList();

            return viewModel;
        }

        public bool IsUserTeacherToSchool(string userId, int schoolId)
        {
            return this.teacherRepository.All().Any(x => x.SchoolId == schoolId && x.ApplicationUserId == userId);
        }
    }
}
