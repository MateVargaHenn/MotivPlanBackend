using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Persistence.Extensions;

namespace MotivPlanBackend.Persistence.Seeders;

public static class WorkoutStatusSeeder
{
    public static async Task SeedWorkoutStatusAsync(IMotivPlanDbContext context)
    {
        var enumValues = Enum.GetValues<WorkoutStatus>();
        var dbValues = await context.WorkoutStatus.ToListAsync();

        // insert
        var missing = enumValues
            .Where(x => dbValues.All(y => y.Id != x));

        foreach (var value in missing)
        {
            context.WorkoutStatus.Add(new WorkoutStatusEntity
            {
                Id = value,
                Name = value.GetDescription()
            });
        }

        // update
        foreach (var entity in dbValues)
        {
            var match = enumValues.FirstOrDefault(x => x == entity.Id);

            if (match.Equals(default(WorkoutStatus)) &&
                !enumValues.Contains(entity.Id))
            {
                continue;
            }

            var description = entity.Id.GetDescription();

            if (entity.Name != description)
            {
                entity.Name = description;
            }
        }

        // delete
        var removed = dbValues
            .Where(x => !enumValues.Contains(x.Id));

        context.WorkoutStatus.RemoveRange(removed);

        await context.SaveChangesAsync();
    }
}
