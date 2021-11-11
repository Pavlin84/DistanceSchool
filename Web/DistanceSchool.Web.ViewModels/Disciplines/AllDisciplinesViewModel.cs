namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System.Collections.Generic;

    public class AllDisciplinesViewModel
    {
        public AllDisciplinesViewModel()
        {
            this.DisciplinesId = new HashSet<int>();
            this.Disciplines = new HashSet<DisciplineViewModel>();
        }

        public ICollection<int> DisciplinesId { get; set; }

        public ICollection<DisciplineViewModel> Disciplines { get; set; }
    }
}
