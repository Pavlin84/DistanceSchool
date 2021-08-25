namespace DistanceSchool.Data.Models
{
    using System;

    using DistanceSchool.Data.Common.Models;

    public class StudentExam : BaseDeletableModel<int>
    {

        public string StudentId { get; set; }

        public Student Student { get; set; }

        public string ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public DateTime ExamDate { get; set; }

        public Evaluation? Evaluation { get; set; }
    }
}
