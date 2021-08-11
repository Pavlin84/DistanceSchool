namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DistanceSchool.Data.Common.Repositories;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Web.ViewModels.Disciplines;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;

    public class TeacherService : ITeacherService
    {
        private readonly IDeletableEntityRepository<Teacher> teacherRepository;
        private readonly IDeletableEntityRepository<Candidacy> candidacyRepository;
        private readonly IDeletableEntityRepository<School> schoolRepository;
        private readonly IDeletableEntityRepository<DisciplineTeacher> disciplineTeacherRepository;

        public TeacherService(
            IDeletableEntityRepository<Teacher> teacherRepository,
            IDeletableEntityRepository<Candidacy> candidacyRepository,
            IDeletableEntityRepository<School> schoolRepository,
            IDeletableEntityRepository<DisciplineTeacher> disciplineTeacherRepository)
        {
            this.teacherRepository = teacherRepository;
            this.candidacyRepository = candidacyRepository;
            this.schoolRepository = schoolRepository;
            this.disciplineTeacherRepository = disciplineTeacherRepository;
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

        public int GetSchoolId(string userId)
        {
            var schoolId = this.teacherRepository.All()
                 .Where(x => x.ApplicationUserId == userId)
                 .Select(x => x.SchoolId)
                 .FirstOrDefault();
            return schoolId;
        }

        public OneTeacherViewModel GetTeacherData(string id, string userId)
        {
            var viewModel = this.teacherRepository.All()
                .Where(x => x.Id == id)
                .Select(x => new OneTeacherViewModel
                {
                    Id = id,
                    TeacherNames = x.FirstName + " " + x.SecondName.Remove(1) + "." + " " + x.LastName,
                    SchoolName = x.School.Name,
                    IsUserManager = x.School.ManagerId == userId,
                    Disciplines = x.DisciplineTeachers.Select(y => new DisciplinesForOneTeacherViewModel
                    {
                        Name = y.Discipline.Name,
                        Teams = y.Discipline.TeacherTeams.Where(z => z.TeacherId == id).Select(z => new TeamBaseViewModel
                        {
                            Id = z.Team.Id,
                            TeamName = z.Team.Name,
                        }).ToList(),
                    }).ToList(),
                }).FirstOrDefault();

            return viewModel;
        }

        public DisciplineHandlerViewModel GetTeacherDisciplines(string teacherId)
        {
            var disciplinies = this.schoolRepository.All()
                .Where(x => x.Teachers.Any(y => y.Id == teacherId))
                .Select(x => new DisciplineHandlerViewModel
                {
                    Id = teacherId,
                    Displines = x.SchoolDisciplines.Select(y => new DisciplineViewModel
                    {
                        Id = y.DisciplineId,
                        Name = y.Discipline.Name,
                        IsStudied = y.Discipline.DisciplineTeachers.Any(z => z.TeacherId == teacherId),
                    }).ToList(),

                }).FirstOrDefault();

            return disciplinies;
        }

        public async Task AddDisciplinesToTeacherAsync(AddDisciplineToTeacherInputModel inputModel)
        {

            var disciplines = this.disciplineTeacherRepository.All().Where(x => x.TeacherId == inputModel.TeacherId).ToList();

            foreach (var discipline in disciplines)
            {
                if (!inputModel.DisciplinesId.Contains(discipline.DisciplineId))
                {
                    discipline.IsDeleted = true;
                }
            }

            foreach (var disciplineId in inputModel.DisciplinesId)
            {
                if (!disciplines.Any(x => x.DisciplineId == disciplineId))
                {
                    await this.disciplineTeacherRepository.AddAsync(new DisciplineTeacher { DisciplineId = disciplineId, TeacherId = inputModel.TeacherId });
                }
            }

            await this.disciplineTeacherRepository.SaveChangesAsync();
        }

        public string GetTeacherId(string userId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == userId);
            return teacher.Id;
        }
    }
}
