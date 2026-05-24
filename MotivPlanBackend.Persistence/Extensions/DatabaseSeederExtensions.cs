using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Persistence.Database;
using MotivPlanBackend.Persistence.Seeders;

namespace MotivPlanBackend.Persistence.Extensions;

public static class DatabaseSeederExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app?.Services.CreateScope();

        var dbContext = scope?.ServiceProvider
            .GetRequiredService<MotivPlanDbContext>();

        if (dbContext is not null)
        {
            await WorkoutStatusSeeder.SeedWorkoutStatusAsync(dbContext);
            await RoleSeeder.SeedRoleAsync(dbContext);
            await SexSeeder.SeedSexAsync(dbContext);
        }
    }
}