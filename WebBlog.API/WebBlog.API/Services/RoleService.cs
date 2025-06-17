using Microsoft.AspNetCore.Identity;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Models;

namespace WebBlog.API.Services
{
    public class RoleService
    {
        public static async Task CreateRoles(IServiceProvider services)
        {
            using var scope = services.CreateScope(); // Create a new scope to obtain scoped services from the DI container 
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleService>>();//

            try
            {
                await context.Database.EnsureCreatedAsync();
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");

                var adminEmail = "adminEmail";
                var admin = new AppUser
                {
                    UserName = "admin3",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    NormalizedEmail = adminEmail.ToUpper(),
                    NormalizedUserName = adminEmail.ToUpper(),
                    Role = "Admin",
                };
                var result = await userManager.CreateAsync(admin, "Admin@13082003");
                if (result.Succeeded)
                {
                    logger.LogInformation("Admin created");
                    await userManager.AddToRoleAsync(admin, "Admin");
                    logger.LogInformation("Admin role added to user");
                }
                else
                {
                    logger.LogError("Faile to create admin user: {Errors}", string.Join(", ", result.Errors.Select(x => x.Description)));


                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while seeding the database");
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}");
                }
            }
        }
    }
}
