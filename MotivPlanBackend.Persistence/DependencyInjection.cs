using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Persistence.Database;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        Ensure.NotNull(configuration);
        return services
        .AddServices()
        .AddDatabase(configuration)
        .AddHealthCheck(configuration);
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMotivPlanDbContext, MotivPlanDbContext>();
        return services;
    }
    private static IServiceCollection AddDatabase(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        var connectionString =
            configuration.GetSection("MotivPlan:Db:PostGres")
                         .GetValue<string>("ConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("PostgreSQL connection string is missing.");

        services.AddDbContext<MotivPlanDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        Schemas.Default));
            },
            ServiceLifetime.Transient,
            ServiceLifetime.Singleton);

        services.AddScoped<IMotivPlanDbContext, MotivPlanDbContext>();

        services.AddDbContextFactory<MotivPlanDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        Schemas.Default));
            });

        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MotivPlanDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetSection("MotivPlan:Db:PostGres")
                         .GetValue<string>("ConnectionString");

        services
            .AddHealthChecks()
            .AddNpgSql(connectionString!);

        return services;
    }
}
