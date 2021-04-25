namespace DistanceSchool.Data.Models
{
    using System;
    using System.Collections.Generic;

    using DistanceSchool.Data.Common.Models;

    public class Exam : BaseDeletableModel<string>
    {
        public Exam()
        {
            this.Id = Guid.NewGuid().ToString();
            this.StudentExams = new HashSet<StudentExam>();
        }

        public string Name { get; set; }

        public TeamLevel Level { get; set; }

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }

        public ICollection<StudentExam> StudentExams { get; set; }
    }
}
