using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Application.Features.Workout;
using MotivPlanBackend.Infrastructure.Time;
using MotivPlanBackend.Persistence.Database;
using MotivPlanBackend.Shared.Common;
using MotivPlanBackend.Test.Dispatchers;

namespace MotivPlanBackend.Test.IntegrationTests;

internal static class IntegrationTestFactory
{
    public static ServiceProvider Create()
    {
        var services = new ServiceCollection();
        services.AddScoped<IDomainEventsDispatcher, TestDomainEventsDispatcher>();

        services.AddDbContext<MotivPlanDbContext>(options =>
            options.UseNpgsql("Host=localhost; Database=motivplan; Username=motivplan; Password=motivplan"));

        services.AddScoped<IMotivPlanDbContext>(sp =>
            sp.GetRequiredService<MotivPlanDbContext>());

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<GetWorkoutByIdQueryHandler>();
        services.AddScoped<SetWorkoutStatusCommandHandler>();

        return services.BuildServiceProvider();
    }
}
