using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace QuizManager.Models
{
    public static class SeedRoles
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[]
            {
                "Edit",
                "View",
                "Restricted"
            };

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var create = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!create.Succeeded)
                    {
                        throw new Exception("Failed to create role");
                    }
                }
        }
    }
}