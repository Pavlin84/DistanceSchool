namespace DistanceSchool.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DistanceSchool.Common;
    using DistanceSchool.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
           var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

           await this.SeedAdminAsync(userManager, dbContext);
        }

        private async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            var role = dbContext.Roles.FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName && x.IsDeleted == false);

            if (dbContext.Users.Any(x => x.Roles.Any(y => y.RoleId == role.Id)))
            {
                return;
            }

            var user = new ApplicationUser { UserName = GlobalConstants.AdministratorName, Email = GlobalConstants.AdministratorEmail };
            await userManager.CreateAsync(user, GlobalConstants.AdministratorPassword);

            var adminUser = dbContext.Users.FirstOrDefault(x => x.UserName == GlobalConstants.AdministratorName);
            await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
        }
    }
}
