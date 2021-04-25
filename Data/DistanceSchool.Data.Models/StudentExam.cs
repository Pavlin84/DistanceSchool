namespace DistanceSchool.Data.Models
{
    using System;

    public class StudentExam
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        public Student Student { get; set; }

        public string ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public DateTime ExamDate { get; set; }

        public Evaluation Evaluation { get; set; }
    }
}
