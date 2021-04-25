namespace DistanceSchool.Data.Models
{
    public class DisciplineTeacher
    {
        public int Id { get; set; }

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }

        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}
