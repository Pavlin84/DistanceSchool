namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Disciplines;

    public class DisciplineHandlerViewModel
    {
        public string Id { get; set; }

        public virtual ICollection<DisciplineViewModel> Displines { get; set; }
    }
}
