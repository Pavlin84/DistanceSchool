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
    using DistanceSchool.Web.ViewModels.Schools;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;

    public class SchoolService : ISchoolService
    {
        private readonly IDeletableEntityRepository<School> schoolRepository;
        private readonly ITeacherServisce teacherServisce;
        private readonly ICandidacyServices candidacyService;
        private readonly IDisciplineService disciplineService;

        public SchoolService(
            IDeletableEntityRepository<School> schoolRepository,
            ITeacherServisce teacherServisce,
            ICandidacyServices candidacyService,
            IDisciplineService disciplineService)
        {
            this.schoolRepository = schoolRepository;
            this.teacherServisce = teacherServisce;
            this.candidacyService = candidacyService;
            this.disciplineService = disciplineService;
        }

        public async Task<int> AddManagerAsync(int candidacyId)
        {
            var managerDto = await this.teacherServisce.CreateTeacherAsync(candidacyId);
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == managerDto.SchoolId);

            // If User is manger set old scholl manager to null
            var oldSchool = this.schoolRepository.All().FirstOrDefault(x => x.ManagerId == managerDto.ManagerId);
            if (oldSchool != null)
            {
                oldSchool.ManagerId = null;
            }

            school.ManagerId = managerDto.ManagerId;

            await this.candidacyService.DeleteAllSchoolMangerCandidacyAsync(managerDto.SchoolId);
            await this.teacherServisce.CahngeSchoolIdAsync(managerDto.ManagerId, managerDto.SchoolId);
            await this.schoolRepository.SaveChangesAsync();

            return school.Id;
        }

        public async Task AddSchoolAsync(AddSchoolInputModel inputModel)
        {
            var school = new School
            {
                Name = inputModel.Name.Trim(),
                Description = inputModel.Description,
                Address = inputModel.Address,
            };

            await this.schoolRepository.AddAsync(school);
            await this.schoolRepository.SaveChangesAsync();
        }

        public List<AllScholsViewModel> AllSchool()
        {
            var schools = this.schoolRepository.All()
                .OrderBy(x => x.Name)
                .Select(x => new AllScholsViewModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Address = x.Address,
                    Description = x.Description,
                    Manager = x.Manager.Teacher.FirstName + " " + x.Manager.Teacher.LastName,
                }).ToList();

            return schools;
        }

        public OneSchoolViewModel GetSchoolData(int schoolId)
        {
            var techer = new TeacherForOneSchoolViewModel
            {
                Id = "TecherId",
                Name = "TecherName",
                Disciplines = new List<string>
                {
                    "Discipine1",
                    "Discipine2",
                    "Discipine3",
                },
            };
            var discipline = new DisciplineForOneSchoolViewModel
            {
                Id = "DiscId",
                Name = "DisciplineName",
                Techers = new List<string>
                {
                    "Teacher1",
                    "Teacher2",
                    "Teacher3",
                },
            };

            var team = new TeamForOneSchoolViewModel
            {
                Id = "TeamId",
                TeamName = "TeamName",
                TecherWhithDisciplines = new List<string>
                {
                    "Discipine1 - Iwan Ivanow",
                    "Discipine2 - Peatar Petrow",
                    "Discipine3 - TestName",
                },
            };

            var school = this.schoolRepository.All()
                .Where(x => x.Id == schoolId)
                .Select(x => new OneSchoolViewModel
                {
                    Id = x.Id.ToString(),
                    Description = x.Description,
                    Manager = new TeacherForOneSchoolViewModel
                    {
                        Name = x.Manager.Teacher.FirstName + " " + x.Manager.Teacher.LastName,
                    },
                    NotStudiedDisciplines = this.disciplineService.GetNotStudiedDisciplines(x.Id),
                    Disciplines = x.SchoolDisciplines.Select(y => new DisciplineForOneSchoolViewModel
                    {
                        Name = y.Discipline.Name,
                        Id = y.DisciplineId.ToString(),
                        Techers = y.Discipline.DisciplineTeachers
                            .Where(s => s.Teacher.SchoolId == schoolId)
                            .Select(d => d.Teacher.FirstName + " " + d.Teacher.LastName)
                            .ToList(),
                    }).ToList(),
                    Name = x.Name,
                }).FirstOrDefault();

            for (int i = 0; i < 5; i++)
            {
                school.Teacher.Add(techer);
                school.Teams.Add(team);
            }

            return school;
        }

        public string GetSchoolName(int id)
        {
            var school = this.schoolRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (school == null)
            {
                return null;
            }

            return school.Name;
        }

        public bool IsUserManger(string userId, int schoolId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == schoolId);

            return school.ManagerId == userId;
        }

        public async Task RemoveManagerAsync(int schoolId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == schoolId);
            school.ManagerId = null;

            await this.schoolRepository.SaveChangesAsync();
        }
    }
}
