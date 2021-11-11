namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System.Collections.Generic;

    using DistanceSchool.Web.ViewModels.Teams;

    public class StudentCandidacyViewModel
    {
        public int SchoolId { get; set; }

        public string SchoolManager { get; set; }

        public string SchoolName { get; set; }

        public ICollection<TeamsCandidacyViewModel> Teams { get; set; }

        public int CurentPage { get; set; }

        public int LastPage { get; set; }

        public int SecondPage => this.CurentPage + 1;

        public int ThirdPage => this.CurentPage + 2;

        public int PreviusPage => this.CurentPage - 1;
    }
}
