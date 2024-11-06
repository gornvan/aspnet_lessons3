using Microsoft.AspNetCore.Identity;

namespace SynopticumIdentityServer.Data.DbSeed
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider services)
        {
            var servicesScope = services.CreateScope();
            var roleManager = servicesScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = servicesScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedRoles(roleManager);
            await SeedUsers(userManager);

        }

        private static readonly string[] roles = new[] { "Admin", "User" };
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in roles)
            {
                // Check if the role already exists
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // Create the role if it doesn't exist
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Seed Admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            if (! userManager.Users.Any(u => u.UserName == adminUser.UserName))
            {
                var result = await userManager.CreateAsync(adminUser, "123!@#qweQWE");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed standard User
            var standardUser = new ApplicationUser
            {
                UserName = "user@example.com",
                Email = "user@example.com",
                EmailConfirmed = true
            };

            if (! userManager.Users.Any(u => u.UserName == standardUser.UserName))
            {
                var result = await userManager.CreateAsync(standardUser, "123!@#qweQWE");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(standardUser, "User");
                }
            }
        }
    }
}
