using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace QuizManager.Models
{
    public class SeedUsers
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<QuizManagerUser>>();
            var editEmail = "edit@email.com";
            var viewEmail = "view@email.com";
            var restrictedEmail = "restricted@email.com";
            

            var existingEditUser = await userManager.FindByEmailAsync(editEmail);
            var existingViewUser = await userManager.FindByEmailAsync(viewEmail);
            var existingRestrictedUser = await userManager.FindByEmailAsync(restrictedEmail);

            if (existingEditUser == null)
            {
                var editUser = new QuizManagerUser()
                {
                    FirstName = "Edit",
                    LastName = "User",
                    EmailConfirmed = true,
                    Email = editEmail,
                    UserName = "Edit"
                };

                var editCreate = await userManager.CreateAsync(editUser, "P@ssword1");
                if (editCreate.Succeeded)
                {
                    await userManager.AddToRoleAsync(editUser, "Edit");
                }
            } 

            if (existingViewUser == null)
            {
                var viewUser = new QuizManagerUser()
                {
                    FirstName = "View",
                    LastName = "User",
                    EmailConfirmed = true,
                    Email = viewEmail,
                    UserName = "View"
                };

                var viewCreate = await userManager.CreateAsync(viewUser, "P@ssword1");
                if (viewCreate.Succeeded)
                {
                    await userManager.AddToRoleAsync(viewUser, "View");
                }
            }
            
            if (existingRestrictedUser == null)
            {
                var restrictedUser = new QuizManagerUser()
                {
                    FirstName = "Restricted",
                    LastName = "User",
                    EmailConfirmed = true,
                    Email = restrictedEmail,
                    UserName = "Restricted"
                };

                var restrictedCreate = await userManager.CreateAsync(restrictedUser, "P@ssword1");
                if (restrictedCreate.Succeeded)
                {
                    await userManager.AddToRoleAsync(restrictedUser, "Restricted");
                }
            }
        }
    }
}
