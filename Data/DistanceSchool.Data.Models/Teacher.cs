namespace DistanceSchool.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DistanceSchool.Data.Common.Models;

    public class Teacher : BaseDeletableModel<string>
    {
        public Teacher()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DisciplineTeachers = new HashSet<DisciplineTeacher>();
            this.TeacherTeams = new HashSet<TeacherTeam>();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public virtual ICollection<DisciplineTeacher> DisciplineTeachers { get; set; }

        public virtual ICollection<TeacherTeam> TeacherTeams { get; set; }

    }
}
