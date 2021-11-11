namespace DistanceSchool.Web.ViewModels.Lessons
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using DistanceSchool.Data.Models;
    using DistanceSchool.Services.Mapping;

    public class AddLessonViewModel : AddLessonBaseViewModel, IHaveCustomMappings
    {
        public AddLessonViewModel()
        {
            this.LessonsName = new List<string>();
            this.LessonsId = new List<int>();
        }

        [Display(Name = "Избери урок")]
        public int Lesson { get; set; }

        public List<int> LessonsId { get; set; }

        public List<string> LessonsName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<TeacherTeam, AddLessonViewModel>()
                .ForMember(x => x.LessonsName, opt => opt.MapFrom(x => x.Discipline.Lessons
                    .Where(l => l.Level == x.Team.Level && l.StudentLessons.All(s => s.Student.TeamId != x.TeamId))
                    .Select(y => y.Name)))
                 .ForMember(x => x.LessonsId, opt => opt.MapFrom(x => x.Discipline.Lessons
                    .Where(l => l.Level == x.Team.Level && l.StudentLessons.All(s => s.Student.TeamId != x.TeamId))
                    .Select(y => y.Id)));
        }
    }
}
