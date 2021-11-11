namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AddDisciplineToTeacherInputModel
    {
        public AddDisciplineToTeacherInputModel()
        {
            this.DisciplinesId = new HashSet<int>();
        }

        public string TeacherId { get; set; }

        public ICollection<int> DisciplinesId { get; set; }
    }
}
