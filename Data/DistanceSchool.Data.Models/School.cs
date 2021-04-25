namespace DistanceSchool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using DistanceSchool.Data.Common.Models;

    public class School : BaseDeletableModel<int>
    {
        public School()
        {
            this.SchoolDisciplines = new HashSet<SchoolDiscipline>();
            this.Teams = new HashSet<Team>();
            this.Teachers = new HashSet<Teacher>();
            this.Candidacies = new HashSet<Candidacy>();
        }

        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        public virtual ICollection<SchoolDiscipline> SchoolDisciplines { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string ManagerId { get; set; }

        public virtual ApplicationUser Manager { get; set; }

        public virtual ICollection<Candidacy> Candidacies { get; set; }
    }
}
