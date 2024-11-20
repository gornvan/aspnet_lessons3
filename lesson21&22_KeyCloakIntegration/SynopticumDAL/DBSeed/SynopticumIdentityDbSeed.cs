using Microsoft.AspNetCore.Identity;
using SynopticumWebApp.Data.Entities;

namespace SynopticumDAL.DBSeed
{
    public class SynopticumIdentityDbSeed
    {
        private readonly UserManager<SynopticumUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SynopticumIdentityDbSeed(
            UserManager<SynopticumUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            const string adminRoleName = "Administrator";
            const string adminUserName = "admin@example.com";
            const string adminPassword = "Admin@123"; // Make sure this meets your password policy

            // Ensure Admin role exists
            if (!await _roleManager.RoleExistsAsync(adminRoleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            // Ensure Admin user exists
            var adminUser = await _userManager.FindByEmailAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new SynopticumUser
                {
                    UserName = adminUserName,
                    Email = adminUserName,
                    EmailConfirmed = true
                };

                var createUserResult = await _userManager.CreateAsync(adminUser, adminPassword);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create Admin user: {string.Join(", ", createUserResult.Errors)}");
                }
            }

            // Ensure Admin user is in Admin role
            if (!await _userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                await _userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
}
