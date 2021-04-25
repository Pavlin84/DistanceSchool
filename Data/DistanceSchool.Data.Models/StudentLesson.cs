namespace DistanceSchool.Data.Models
{
    using System;

    public class StudentLesson
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int LessonId { get; set; }

        public virtual Lesson Lesson { get; set; }

        public DateTime StudyDate { get; set; }

        public bool IsAttended { get; set; }
    }
}
