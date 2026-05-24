using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;

internal class IdentityModelBuilder
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>().ToTable("Users", "identity");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles", "identity");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");
    }
}
