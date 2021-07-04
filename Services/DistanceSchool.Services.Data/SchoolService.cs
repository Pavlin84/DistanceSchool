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
    using DistanceSchool.Web.ViewModels.Managers;
    using DistanceSchool.Web.ViewModels.Schools;
    using DistanceSchool.Web.ViewModels.Teachers;
    using DistanceSchool.Web.ViewModels.Teams;

    public class SchoolService : ISchoolService
    {
        private readonly IDeletableEntityRepository<School> schoolRepository;
        private readonly ITeacherService teacherService;
        private readonly ICandidacyServices candidacyService;
        private readonly IDisciplineService disciplineService;

        public SchoolService(
            IDeletableEntityRepository<School> schoolRepository,
            ITeacherService teacherServisce,
            ICandidacyServices candidacyService,
            IDisciplineService disciplineService)
        {
            this.schoolRepository = schoolRepository;
            this.teacherService = teacherServisce;
            this.candidacyService = candidacyService;
            this.disciplineService = disciplineService;
        }

        public async Task<int> AddManagerAsync(int candidacyId)
        {
            var managerDto = await this.teacherService.CreateTeacherAsync(candidacyId);

            // If User is manger set old scholl manager to null
            await this.CheckAndDeleteOldSchoolManager(managerDto.UserId);

            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == managerDto.SchoolId);

            school.ManagerId = managerDto.UserId;

            await this.candidacyService.DeleteAllSchoolMangerCandidacyAsync(managerDto.SchoolId);
            await this.teacherService.ChangeSchoolIdAsync(managerDto.UserId, managerDto.SchoolId);
            await this.schoolRepository.SaveChangesAsync();

            return school.Id;
        }

        public async Task CheckAndDeleteOldSchoolManager(string userId)
        {
            var oldSchool = this.schoolRepository.All().FirstOrDefault(x => x.ManagerId == userId);
            if (oldSchool != null)
            {
                oldSchool.ManagerId = null;
            }

            await this.schoolRepository.SaveChangesAsync();
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
            var techers = this.teacherService.GetAllTeacherFromSchool(schoolId);

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

            foreach (var teacher in techers)
            {
                school.Teachers.Add(teacher);
            }

            for (int i = 0; i < 5; i++)
            {
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

        public bool IsUserMangerToSchool(string userId, int schoolId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == schoolId);

            return school.ManagerId == userId;
        }

        public bool IsUserManger(string userId)
        {
            var isManager = this.schoolRepository.All().Any(x => x.ManagerId == userId);

            return isManager;
        }


        public async Task RemoveManagerAsync(int schoolId)
        {
            var school = this.schoolRepository.All().FirstOrDefault(x => x.Id == schoolId);
            school.ManagerId = null;

            await this.schoolRepository.SaveChangesAsync();
        }

        public int GetSchoolIdWithManager(string managerId)
        {
            var schoolId = this.schoolRepository.All()
                .Where(x => x.ManagerId == managerId)
                .Select(x => x.Id)
                .FirstOrDefault();

            return schoolId;
        }

        public SchoolManagerHomeViewModel GetManagerHomePageData(string managerId, string directoryPath)
        {
            var schoolId = this.GetSchoolIdWithManager(managerId);
            return this.GetManagerHomePageBySchholId(schoolId, directoryPath);
        }

        public SchoolManagerHomeViewModel GetManagerHomePageBySchholId(int schoolId, string directoryPath)
        {
            return this.schoolRepository.All()
                            .Where(x => x.Id == schoolId)
                            .Select(x => new SchoolManagerHomeViewModel
                            {
                                SchoolName = x.Name,
                                SchoolManager = x.Manager.Teacher.FirstName + " " + x.Manager.Teacher.LastName,
                                Candidacies = this.candidacyService.GetSchoolCandidacies(schoolId, directoryPath, CandidacyType.Teacher),
                            }).FirstOrDefault();
        }

        public async Task<string> AddTeacherToSchool(int candidacyId)
        {
            var teacherDto = await this.teacherService.CreateTeacherAsync(candidacyId);
            await this.CheckAndDeleteOldSchoolManager(teacherDto.UserId);
            await this.teacherService.ChangeSchoolIdAsync(teacherDto.UserId, teacherDto.SchoolId);
            await this.candidacyService.DeleteCandicayAsync(candidacyId);

            return teacherDto.TeacherId;
        }
    }
}
