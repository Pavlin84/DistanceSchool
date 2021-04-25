namespace DistanceSchool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Data.Common.Models;

    public class Team : BaseDeletableModel<int>
    {
        public Team()
        {
            this.Students = new HashSet<Student>();
            this.TeacherTeams = new HashSet<TeacherTeam>();
            this.Candidacies = new HashSet<Candidacy>();
        }

        [Required]
        [MaxLength(8)]
        public string Name { get; set; }

        public TeamLevel Level { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public int MyProperty { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<TeacherTeam> TeacherTeams { get; set; }

        public virtual ICollection<Candidacy> Candidacies { get; set; }
    }
}
