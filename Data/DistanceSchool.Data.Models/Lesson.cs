namespace DistanceSchool.Data.Models
{
    using System.Collections.Generic;

    using DistanceSchool.Data.Common.Models;

    public class Lesson : BaseDeletableModel<int>
    {
        public Lesson()
        {
            this.StudentLessons = new HashSet<StudentLesson>();
        }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public TeamLevel Level { get; set; }

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }

        public virtual ICollection<StudentLesson> StudentLessons { get; set; }
    }
}
