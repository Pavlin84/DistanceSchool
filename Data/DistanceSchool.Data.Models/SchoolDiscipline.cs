namespace DistanceSchool.Data.Models
{
    public class SchoolDiscipline
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }
    }
}
