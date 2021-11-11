namespace DistanceSchool.Web.ViewModels.Disciplines
{
    using System.Collections.Generic;

    public class AddDisciplinesToTeamInputModel
    {
        public AddDisciplinesToTeamInputModel()
        {
            this.DisciplinesId = new HashSet<int>();
        }

        public int TeamId { get; set; }

        public ICollection<int> DisciplinesId { get; set; }
    }
}
