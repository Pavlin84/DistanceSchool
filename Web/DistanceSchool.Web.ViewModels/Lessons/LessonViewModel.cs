namespace DistanceSchool.Web.ViewModels.Lessons
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LessonViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsStudied { get; set; }

        public DateTime DateOfStudy { get; set; }
    }
}
