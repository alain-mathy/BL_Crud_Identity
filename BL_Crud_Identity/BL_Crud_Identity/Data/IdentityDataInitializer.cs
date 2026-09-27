using Microsoft.AspNetCore.Identity;

namespace BL_Crud_Identity.Data
{
    /// <summary>
    /// Provides automated data seeding functions for Identity roles and default administrative users.
    /// </summary>
    public static class IdentityDataInitializer
    {
        /// <summary>
        /// Checks for default roles and creates a master administrator account if it does not exist.
        /// </summary>
        /// <param name="userManager">The Identity user manager instance.</param>
        /// <param name="roleManager">The Identity role manager instance.</param>
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Seed core system roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed default Administrator credentials
            const string adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    Street = "Tech Avenue 1",
                    ZipCode = "1000",
                    City = "Brussels",
                    EmailConfirmed = true
                };

                // Enforces the password policies defined in your server's Program.cs
                var createPowerUser = await userManager.CreateAsync(newAdmin, "Test1234?");
                if (createPowerUser.Succeeded)
                {
                    // Tie the newly created account to the Admin role scheme
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
