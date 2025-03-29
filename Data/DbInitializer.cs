using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Models;
using System;
using System.Threading.Tasks;

namespace NepHubAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                
                // Check if admin user already exists
                var adminUser = await userManager.FindByNameAsync("admin");
                
                if (adminUser == null)
                {
                    // Create admin user
                    adminUser = new AppUser
                    {
                        UserName = "admin",
                        Email = "admin@nephub.com",
                        EmailConfirmed = true
                    };

                    // Create admin with password
                    var result = await userManager.CreateAsync(adminUser, "Admin@123456");
                    
                    if (result.Succeeded)
                    {
                        // Add user to admin role
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }
        }
    }
}