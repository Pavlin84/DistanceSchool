namespace DistanceSchool.Web.ViewModels.Teams
{
    using System.Collections.Generic;

    public class AddDisciplineViewModel : TeamBaseViewModel
    {
        public ICollection<string> StudyDisciplines { get; set; }

        public ICollection<string> NotStudyDisciplines { get; set; }
    }
}
