namespace DistanceSchool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Data.Common.Models;

    public class Discipline : BaseDeletableModel<int>
    {
        public Discipline()
        {
            this.SchoolDisciplines = new HashSet<SchoolDiscipline>();
            this.DisciplineTeachers = new HashSet<DisciplineTeacher>();
            this.Exams = new HashSet<Exam>();
            this.Lessons = new HashSet<Lesson>();
            // this.Candidacies = new HashSet<Candidacy>();
            this.TeacherTeams = new HashSet<TeacherTeam>();
        }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public virtual ICollection<SchoolDiscipline> SchoolDisciplines { get; set; }

        public virtual ICollection<DisciplineTeacher> DisciplineTeachers { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }

        // public virtual ICollection<Candidacy> Candidacies { get; set; }

        public virtual ICollection<TeacherTeam> TeacherTeams { get; set; }
    }
}
