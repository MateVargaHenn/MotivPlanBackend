using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MotivPlanBackend.Persistence.Seeders;

public static class IdentitySeeder
{
    public static class AppRoles
    {
        public const string SuperAdministrator = "SuperAdministrator";
        public const string Administrator = "Administrator";
        public const string User = "User";
    }
    public static async Task SeedSuperAdministratorAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        await EnsureRoleExistsAsync(roleManager, AppRoles.SuperAdministrator);
        await EnsureRoleExistsAsync(roleManager, AppRoles.Administrator);
        await EnsureRoleExistsAsync(roleManager, AppRoles.User);

        var email = Environment.GetEnvironmentVariable("APP_SUPERADMIN_EMAIL");
        var userName = Environment.GetEnvironmentVariable("APP_SUPERADMIN_USERNAME");
        var password = Environment.GetEnvironmentVariable("APP_SUPERADMIN_PASSWORD");

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("SuperAdministrator seed configuration is missing.");
        }

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser is null)
        {
            var user = new IdentityUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create SuperAdministrator user: {errors}");
            }

            existingUser = user;
        }

        if (!await userManager.IsInRoleAsync(existingUser, AppRoles.SuperAdministrator))
        {
            var roleResult = await userManager.AddToRoleAsync(existingUser, AppRoles.SuperAdministrator);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign SuperAdministrator role: {errors}");
            }
        }
    }
    private static async Task EnsureRoleExistsAsync(
        RoleManager<IdentityRole> roleManager,
        string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
            }
        }
    }
}
