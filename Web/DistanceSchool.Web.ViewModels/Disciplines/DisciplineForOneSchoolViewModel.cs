namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System.Collections.Generic;

    public class DisciplineForOneSchoolViewModel
    {
        public DisciplineForOneSchoolViewModel()
        {
            this.Techers = new List<string>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<string> Techers { get; set; }
    }
}
