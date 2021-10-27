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
    using DistanceSchool.Web.ViewModels.Teams;

    public class DisciplineService : IDisciplineService
    {
        private readonly IDeletableEntityRepository<Discipline> disciplineRepository;
        private readonly IRepository<SchoolDiscipline> schoolDisciplineRepository;
        private readonly IRepository<DisciplineTeacher> disciplineTeacherRepository;
        private readonly IDeletableEntityRepository<School> schoolRepository;

        public DisciplineService(
            IDeletableEntityRepository<Discipline> disciplineRepository,
            IRepository<SchoolDiscipline> schoolDisciplineRepository,
            IRepository<DisciplineTeacher> disciplineTeacherRepository,
            IDeletableEntityRepository<School> schoolRepository)
        {
            this.disciplineRepository = disciplineRepository;
            this.schoolDisciplineRepository = schoolDisciplineRepository;
            this.disciplineTeacherRepository = disciplineTeacherRepository;
            this.schoolRepository = schoolRepository;
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

        public ICollection<string> GetAllDiscplineName()
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
            var disciplineSchool = this.schoolDisciplineRepository.All()
                    .FirstOrDefault(x => x.DisciplineId == disciplineId && x.SchoolId == schoolId);

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

        public Dictionary<string, int> GetSchoolDisciplines(int schoolId)
        {
            var disciplines = this.disciplineRepository.All()
                .Where(x => x.SchoolDisciplines.Any(y => y.SchoolId == schoolId))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();

            var result = new Dictionary<string, int>();

            foreach (var discipline in disciplines)
            {
                result[discipline.Name] = discipline.Id;
            }

            return result;
        }

        public ICollection<DisciplineForOneTeamViewModel> GetTeamDisciplines(int id)
        {
            var disciplines = this.disciplineRepository.All()
                .Where(x => x.TeacherTeams.Any(y => y.TeamId == id))
                .Select(x => new DisciplineForOneTeamViewModel
                {
                    Id = x.TeacherTeams.FirstOrDefault(y => y.TeamId == id).Id,
                    DisciplineName = x.Name,
                    TeacherNames = x.TeacherTeams
                        .Where(y => y.TeamId == id)
                        .Select(x => x.Teacher.FirstName + " " + x.Teacher.LastName)
                        .FirstOrDefault(),
                }).ToList();

            return disciplines;
        }

        public DisciplineHandlerViewModel GetAllDisciplineForTeam(int id)
        {
            var result = this.schoolRepository.All()
                .Where(x => x.Teams.Any(y => y.Id == id))
                .Select(x => new DisciplineHandlerViewModel
                {
                    Id = id.ToString(),
                    Displines = x.SchoolDisciplines.Select(y => new DisciplineViewModel
                    {
                        Id = y.DisciplineId,
                        Name = y.Discipline.Name,
                        IsStudied = y.Discipline.TeacherTeams.Any(z => z.TeamId == id),
                    }).ToList(),
                }).FirstOrDefault();

            return result;
        }

        public AllDisciplinesViewModel GetAllDisciplines()
        {
            var disciplines = this.disciplineRepository.All()
              .OrderByDescending(x => x.Name)
              .Select(x => new DisciplineViewModel
              {
                  Id = x.Id,
                  Name = x.Name,
              }).ToList();
            var viewModel = new AllDisciplinesViewModel();
            viewModel.Disciplines = disciplines;

            return viewModel;
        }
    }
}
