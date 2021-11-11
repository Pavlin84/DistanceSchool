using DistanceSchool.Web.ViewModels.Lessons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DistanceSchool.Services.Data
{
    public interface ILessonService
    {
        T GetDisciplineLesonWithTeacherTeamId<T>(int teacherTeamId);

        Task CreateLessonAsync(string directoryPath, CreateAndAddLessonViewModel inputModel);

        Task AddLessonToTeam(int teamId, int lessonId, DateTime studyDate);
    }
}
