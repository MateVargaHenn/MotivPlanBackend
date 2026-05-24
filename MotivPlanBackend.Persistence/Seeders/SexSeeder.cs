using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Persistence.Seeders;

internal static class SexSeeder
{
    public static async Task SeedSexAsync(IMotivPlanDbContext context)
    {
        var dbValues = await context.Sex.ToListAsync();

        ICollection<SexEntity> sex = [
             new SexEntity {
                 Id = Sex.None,
                 Name = "None",
                 NormalizedName = "NONE",
                 ConcurrencyStamp = Guid.Parse("087f79f6-4be2-4f36-86ae-942fcf5ede99")
             },
             new SexEntity {
                 Id = Sex.Male,
                 Name = "Male",
                 NormalizedName = "MALE",
                 ConcurrencyStamp = Guid.Parse("d58ff2ae-c0fb-4331-9126-9fdbcda958ac")
             },
             new SexEntity {
                 Id = Sex.Female,
                 Name = "Female",
                 NormalizedName = "FEMALE",
                 ConcurrencyStamp = Guid.Parse("3798648b-1225-4476-9cf6-f03ee21a64d6")
             }
         ];


        var missing = sex
            .Where(x => dbValues.All(y => y.NormalizedName != x.NormalizedName));

        foreach (var value in missing)
        {
            context.Sex.Add(new SexEntity
            {
                Id = value.Id,
                Name = value.Name,
                NormalizedName = value.NormalizedName
            });
        }


        // update
        foreach (var entity in dbValues)
        {
            var match = sex.FirstOrDefault(x => x.NormalizedName == entity.NormalizedName);
            if (match is null) continue;

            entity.Name = match.Name;
            entity.NormalizedName = match.NormalizedName;
        }

        // delete
        var removed = dbValues
            .Where(x => !sex.Any(r => r.NormalizedName == x.NormalizedName));

        foreach (var value in removed)
        {
            context.Sex.Remove(value);
        }

        await context.SaveChangesAsync();
    }
}
