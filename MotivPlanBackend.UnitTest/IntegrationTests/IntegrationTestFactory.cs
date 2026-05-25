using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Application.Features.Account;
using MotivPlanBackend.Application.Features.Workout;
using MotivPlanBackend.Application.Managers;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Infrastructure.Time;
using MotivPlanBackend.Persistence.Database;
using MotivPlanBackend.Shared.Common;
using MotivPlanBackend.Test.Dispatchers;
using MotivPlanBackend.Test.Services;

namespace MotivPlanBackend.Test.IntegrationTests;

internal static class IntegrationTestFactory
{
    public static ServiceProvider Create()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDomainEventsDispatcher, TestDomainEventsDispatcher>();

        services.AddDbContext<MotivPlanDbContext>(options =>
            options.UseNpgsql("Host=localhost; Database=motivplan; Username=motivplan; Password=motivplan"));
        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MotivPlanDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IMotivPlanDbContext>(sp =>
            sp.GetRequiredService<MotivPlanDbContext>());

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ProfileManager<ProfileEntity>>();

        services.AddScoped<GetWorkoutByIdQueryHandler>();
        services.AddScoped<SetWorkoutStatusCommandHandler>();
        services.AddScoped<SignUpAccountCommandHandler>();
        services.AddLogging();
        services.AddSingleton<IPublishEndpoint, TestPublishEndpoint>();

        var provider = services.BuildServiceProvider();

        _ = provider.GetRequiredService<UserManager<IdentityUser>>();
        _ = provider.GetRequiredService<SignUpAccountCommandHandler>();
        
        return provider;
    }
}
