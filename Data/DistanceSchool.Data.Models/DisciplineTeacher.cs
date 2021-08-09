namespace DistanceSchool.Data.Models
{
    using DistanceSchool.Data.Common.Models;

    public class DisciplineTeacher : BaseDeletableModel<int>
    {

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }

        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}
