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

    public class DisciplineService : IDisciplineService
    {
        private readonly IDeletableEntityRepository<Discipline> disciplineRepository;
        private readonly IRepository<SchoolDiscipline> schoolDisciplineRepository;
        private readonly IRepository<DisciplineTeacher> disciplineTeacherRepository;

        public DisciplineService(
            IDeletableEntityRepository<Discipline> disciplineRepository,
            IRepository<SchoolDiscipline> schoolDisciplineRepository,
            IRepository<DisciplineTeacher> disciplineTeacherRepository)
        {
            this.disciplineRepository = disciplineRepository;
            this.schoolDisciplineRepository = schoolDisciplineRepository;
            this.disciplineTeacherRepository = disciplineTeacherRepository;
        }

        public async Task CreateDisciplineAsync(CreateDisciplineInputModel inputModel)
        {
            var result = this.disciplineRepository.All().FirstOrDefault(x => x.Name.ToLower() == inputModel.Name.ToLower().Trim());

            if (result != null)
            {
                return;
            }

            var discipline = new Discipline
            {
                Name = inputModel.Name.Trim(),
            };

            await this.disciplineRepository.AddAsync(discipline);
            await this.disciplineRepository.SaveChangesAsync();
        }

        public ICollection<string> GetAllDiscpline()
        {
            var result = this.disciplineRepository.All().OrderBy(x => x.Name).Select(x => x.Name).ToList();

            return result;
        }

        public bool IsExsist(string discipline)
        {
            return this.disciplineRepository.All().Any(x => x.Name.ToLower() == discipline.ToLower());
        }

        public ICollection<DisciplineForOneSchoolViewModel> GetNotStudiedDisciplines(int schoolId)
        {
            var disciplines = this.disciplineRepository.All()
                .Where(x => x.SchoolDisciplines.All(x => x.SchoolId != schoolId))
                .Select(x => new DisciplineForOneSchoolViewModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,

                }).ToList();

            return disciplines;
        }

        public async Task AddDisciplineToSchoolAsync(int disciplineId, int schoolId)
        {
            _ = this.schoolDisciplineRepository.AddAsync(new SchoolDiscipline
            {
                SchoolId = schoolId,
                DisciplineId = disciplineId,
            });

            await this.schoolDisciplineRepository.SaveChangesAsync();
        }

        public async Task RemoveDisciplineFromSchoolAsync(int disciplineId, int schoolId)
        {
            var disciplineSchool = this.schoolDisciplineRepository.All().FirstOrDefault(x => x.DisciplineId == disciplineId && x.SchoolId == schoolId);

            var disciplineTeachers = this.disciplineTeacherRepository.All()
                .Where(x => x.DisciplineId == disciplineId && x.Teacher.SchoolId == schoolId)
                .ToList();

            foreach (var disciplineTeacher in disciplineTeachers)
            {
                this.disciplineTeacherRepository.Delete(disciplineTeacher);
            }

            this.schoolDisciplineRepository.Delete(disciplineSchool);
            await this.schoolDisciplineRepository.SaveChangesAsync();
            await this.disciplineTeacherRepository.SaveChangesAsync();
        }
    }
}
