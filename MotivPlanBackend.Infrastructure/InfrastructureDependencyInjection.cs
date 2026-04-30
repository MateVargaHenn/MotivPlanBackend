using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Infrastructure.DomainEvents;
using MotivPlanBackend.Infrastructure.Time;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddServices();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        return services;
    }
    
}
