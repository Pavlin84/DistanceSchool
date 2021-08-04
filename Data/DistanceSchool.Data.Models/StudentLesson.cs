namespace DistanceSchool.Data.Models
{
    using System;

    using DistanceSchool.Data.Common.Models;

    public class StudentLesson : BaseDeletableModel<int>
    {
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int LessonId { get; set; }

        public virtual Lesson Lesson { get; set; }

        public DateTime StudyDate { get; set; }

        public bool IsAttended { get; set; }
    }
}
