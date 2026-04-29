using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Persistence.Database;

namespace MotivPlanBackend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IHostBuilder hostBuilder) =>
        services
        .AddServices()
        .AddDatabase(hostBuilder);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMotivPlanDbContext, MotivPlanDbContext>();
        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services, IHostBuilder hostBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("MotivPlan:Db:PostGres:ConnectionString");

        services.AddDbContext<MotivPlanDbContext>(
            options =>
            {
                options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default));
            }, ServiceLifetime.Transient, ServiceLifetime.Singleton);

        services.AddScoped<IMotivPlanDbContext, MotivPlanDbContext>();

        services.AddDbContextFactory<MotivPlanDbContext>(
            options =>
            {
                options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default));
            });

        return services;
    }
    private static IServiceCollection AddHealthCheck(this IServiceCollection services)
    {
        string? connectionString = Environment.GetEnvironmentVariable("MotivPlan:Db:PostGres:ConnectionString");

        services
            .AddHealthChecks()
            .AddNpgSql(connectionString!);

        return services;
    }
}
