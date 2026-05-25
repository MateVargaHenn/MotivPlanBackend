using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;

namespace MotivPlanBackend.Persistence.Seeders;

internal static class RoleSeeder
{
    public static async Task SeedRoleAsync(IMotivPlanDbContext context)
    {
        var dbValues = await context.Roles.ToListAsync();
        ICollection<IdentityRole> roles = [             new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SuperAdministrator",
                NormalizedName = "SUPERADMINISTRATOR"
            },
             new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Personal Trainer",
                NormalizedName = "PERSONAL TRAINER"
            }
         ];


        var missing = roles
            .Where(x => dbValues.All(y => y.NormalizedName != x.NormalizedName));

        foreach (var value in missing)
        {
            context.Roles.Add(new IdentityRole
            {
                Id = value.Id,
                Name = value.Name,
                NormalizedName = value.NormalizedName
            });
        }


        // update
        foreach (var entity in dbValues)
        {
            var match = roles.FirstOrDefault(x => x.NormalizedName == entity.NormalizedName);
            if (match is null) continue;

            entity.Name = match.Name;
            entity.NormalizedName = match.NormalizedName;
        }

        // delete
        var removed = dbValues
            .Where(x => !roles.Any(r => r.NormalizedName == x.NormalizedName));

        foreach (var value in removed)
        {
            context.Roles.Remove(value);
        }

        await context.SaveChangesAsync();
    }
}
