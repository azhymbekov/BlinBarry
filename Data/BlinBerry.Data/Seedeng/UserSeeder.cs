using BlinBerry.Data.Models.IdentityModels;
using GlobalContants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlinBerry.Data.Seedeng
{
    public class UserSeeder
    {
        private const string AdminLogin = "kuba";
        private const string Password = "123";

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, context);
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            var user = await userManager.FindByNameAsync(AdminLogin);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = AdminLogin
                };
                var result = await userManager.CreateAsync(user, Password);
                if (!result.Succeeded)
                {
                    return;
                }
                else
                {
                    var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == GlobalConstants.Roles.Kuba);
                    if (role != null)
                    {
                        context.UserRoles.Add(new IdentityUserRole<Guid>
                        {
                            UserId = user.Id,
                            RoleId = role.Id,
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
