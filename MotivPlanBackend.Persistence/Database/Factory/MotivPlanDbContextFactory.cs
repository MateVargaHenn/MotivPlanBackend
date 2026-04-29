using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Persistence.Constants;
using MotivPlanBackend.Persistence.Dispatchers;

namespace MotivPlanBackend.Persistence.Database.Factory;

public class MotivPlanDbContextFactory : IDbContextFactory<MotivPlanDbContext>
{
    public MotivPlanDbContext CreateDbContext()
    {
        string connStr = Environment.GetEnvironmentVariable(DatabaseConfigurationNames.ConnectionString)
                         ?? throw new InvalidOperationException("Environment variable 'MOTIVPLAN_CONNECTION_STRING' is not set.");
        DbContextOptions<MotivPlanDbContext> options = new DbContextOptionsBuilder<MotivPlanDbContext>()
                                                            .UseNpgsql(connStr, x => x.MigrationsAssembly(typeof(MotivPlanDbContext).Assembly.FullName))
                                                            .Options;

        return new MotivPlanDbContext(options, new NoOpDomainEventsDispatcher());
    }
}
