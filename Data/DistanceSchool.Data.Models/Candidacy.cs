namespace DistanceSchool.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DistanceSchool.Data.Common.Models;

    public class Candidacy : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int? SchoolId { get; set; }

        public virtual School School { get; set; }

        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ApplicationDocumentsPath { get; set; }

        public CandidacyType? Type { get; set; }
    }
}
