using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MotivPlanBackend.Persistence.Dispatchers;

namespace MotivPlanBackend.Persistence.Database.Factory;

public class MotivPlanDbContextFactory : IDesignTimeDbContextFactory<MotivPlanDbContext>
{
    public MotivPlanDbContext CreateDbContext(string[] args)
    {
        string connStr = Environment.GetEnvironmentVariable("MotivPlan__Db__PostGres__ConnectionString")
                         ?? throw new InvalidOperationException("Environment variable 'MotivPlan:Db:PostGres:ConnectionString' is not set.");
        DbContextOptions<MotivPlanDbContext> options = new DbContextOptionsBuilder<MotivPlanDbContext>()
                                                            .UseNpgsql(connStr, x => x.MigrationsAssembly(typeof(MotivPlanDbContext).Assembly.FullName))
                                                            .Options;

        return new MotivPlanDbContext(options, new NoOpDomainEventsDispatcher());
    }
}
