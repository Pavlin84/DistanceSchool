namespace DistanceSchool.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DistanceSchool.Data.Common.Models;

    public class Student : BaseDeletableModel<string>
    {
        public Student()
        {
            this.Id = Guid.NewGuid().ToString();
            this.StudentLessons = new HashSet<StudentLesson>();
            this.StudentExams = new HashSet<StudentExam>();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ApplicationDocumentsPath { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public virtual ICollection<StudentLesson> StudentLessons { get; set; }

        public virtual ICollection<StudentExam> StudentExams { get; set; }
    }
}
