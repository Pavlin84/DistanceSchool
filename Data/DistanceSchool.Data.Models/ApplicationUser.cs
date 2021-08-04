// ReSharper disable VirtualMemberCallInConstructor
namespace DistanceSchool.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using DistanceSchool.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Candidacies = new HashSet<Candidacy>();
        }

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public string ApplicationDocumentsExtension { get; set; }

        public string ProfileImageExtension { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
       
        public virtual ICollection<Candidacy> Candidacies { get; set; }
    }
}
