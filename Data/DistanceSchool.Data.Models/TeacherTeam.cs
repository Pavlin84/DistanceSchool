
namespace DistanceSchool.Data.Models
{
    using DistanceSchool.Data.Common.Models;

    public class TeacherTeam : BaseDeletableModel<int>
    {
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }
    }
}
