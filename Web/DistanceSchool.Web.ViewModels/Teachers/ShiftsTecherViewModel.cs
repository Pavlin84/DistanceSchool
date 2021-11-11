using System.Collections.Generic;

namespace DistanceSchool.Web.ViewModels.Teachers
{
    public class ShiftsTecherViewModel
    {
        public ShiftsTecherViewModel()
        {
            this.Teachers = new HashSet<TeacherViewModel>();
        }

        public int TeacherTeamId { get; set; }

        public ICollection<TeacherViewModel> Teachers { get; set; }
    }
}
