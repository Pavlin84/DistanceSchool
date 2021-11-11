namespace DistanceSchool.Web.ViewModels.Candidacyes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CandidacyViewModel : BaseCandidacyModel
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string ProfilPictutreUrl { get; set; }

        public string ApplicationDocumenstUrl { get; set; }

    }
}
