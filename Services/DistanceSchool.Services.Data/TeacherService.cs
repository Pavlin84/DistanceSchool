﻿namespace DistanceSchool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IDeletableEntityRepository<TeacherTeam> teacherTeamRepository;

        public TeacherService(
            IDeletableEntityRepository<Teacher> teacherRepository,
            IDeletableEntityRepository<Candidacy> candidacyRepository,
            IDeletableEntityRepository<School> schoolRepository,
            IDeletableEntityRepository<DisciplineTeacher> disciplineTeacherRepository,
            IDeletableEntityRepository<TeacherTeam> teacherTeamRepository)
        {
            this.teacherRepository = teacherRepository;
            this.candidacyRepository = candidacyRepository;
            this.schoolRepository = schoolRepository;
            this.disciplineTeacherRepository = disciplineTeacherRepository;
            this.teacherTeamRepository = teacherTeamRepository;
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

        public async Task<string> ChangeSchoolIdAsync(string id, int schoolId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == id);

            if (teacher.SchoolId != 0)
            {
                var teacherTeams = this.teacherTeamRepository.All()
                    .Where(x => x.Teacher.ApplicationUserId == id).ToList();

                foreach (var teacherTeam in teacherTeams)
                {
                    teacherTeam.TeacherId = null;
                }

                await this.teacherTeamRepository.SaveChangesAsync();
            }

            teacher.SchoolId = schoolId;

            await this.teacherRepository.SaveChangesAsync();

            return teacher.Id;
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
                    Disciplines = x.DisciplineTeachers
                        .Where(y => y.Discipline.SchoolDisciplines.Any(z => z.SchoolId == schoolId))
                        .Select(x => x.Discipline.Name).ToList(),
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
                    Disciplines = x.DisciplineTeachers
                        .Where(d => d.Discipline.SchoolDisciplines.Any(s => s.SchoolId == x.SchoolId))
                        .Select(y => new DisciplinesForOneTeacherViewModel
                        {
                            Name = y.Discipline.Name,
                            Teams = y.Discipline.TeacherTeams.Where(z => z.TeacherId == id).Select(z => new TeamBaseViewModel
                            {
                                Id = z.Team.Id,
                                TeamName = z.Team.Name + " " + z.Team.Level,
                                TeacherTheamId = z.Id,
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
            var teacherTeams = this.teacherTeamRepository.All().Where(x => x.TeacherId == inputModel.TeacherId).ToList();

            foreach (var discipline in disciplines)
            {
                if (!inputModel.DisciplinesId.Contains(discipline.DisciplineId))
                {
                    foreach (var teacherTeam in teacherTeams.Where(x => x.DisciplineId == discipline.DisciplineId))
                    {
                        teacherTeam.TeacherId = null;
                    }

                    discipline.IsDeleted = true;
                }
            }

            foreach (var disciplineId in inputModel.DisciplinesId)
            {
                if (!disciplines.Any(x => x.DisciplineId == disciplineId))
                {
                    await this.disciplineTeacherRepository
                        .AddAsync(new DisciplineTeacher { DisciplineId = disciplineId, TeacherId = inputModel.TeacherId });
                }
            }

            await this.disciplineTeacherRepository.SaveChangesAsync();
        }

        public string GetTeacherId(string userId)
        {
            var teacher = this.teacherRepository.All().FirstOrDefault(x => x.ApplicationUserId == userId);

            var id = teacher == null ? null : teacher.Id;

            return id;
        }

        public ShiftsTecherViewModel ShiftsTeacher(int teacherTeamId)
        {
            var teacherTeam = this.teacherTeamRepository.All().FirstOrDefault(x => x.Id == teacherTeamId);

            var schoolId = this.schoolRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Teams.Any(y => y.TeacherTeams.Any(z => z.Id == teacherTeamId))).Id;

            var viewModel = new ShiftsTecherViewModel
            {
                TeacherTeamId = teacherTeamId,
                Teachers = this.teacherRepository.AllAsNoTracking()
                .Where(x => x.SchoolId == schoolId
                        && x.DisciplineTeachers.Any(y => y.DisciplineId == teacherTeam.DisciplineId))
                .Select(x => new TeacherViewModel
                {
                    Names = $"{x.FirstName} {x.SecondName} {x.LastName}",
                    Id = x.Id,
                }).ToList(),
            };

            return viewModel;
        }
    }
}
