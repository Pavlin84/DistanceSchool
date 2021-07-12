namespace DistanceSchool.Web.ViewModels.Teachers
{
    using System.Collections.Generic;

    public class TeacherForOneSchoolViewModel
    {
        public TeacherForOneSchoolViewModel()
        {
            this.Disciplines = new List<string>();
        }

        public string UserId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<string> Disciplines { get; set; }
    }
}
