using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.Behaviors;
using MotivPlanBackend.Application.Managers;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationDependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        //.AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
        //    .AsImplementedInterfaces()
        //    .WithScopedLifetime());

        //services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));

        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationDependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<ProfileManager<ProfileEntity>>();

        string licenseKey = Environment.GetEnvironmentVariable("MediatR__LicenseKey")!;
        services.AddMediatR(
            config =>
            {
                config.LicenseKey = licenseKey;
                config.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
            });

        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly, includeInternalTypes: true);
        // Application Services
        return services;
    }
}

/// <summary>
/// Marker type used to reference this assembly for scanning and registration.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1812:Avoid uninstantiated internal classes",
    Justification = "Assembly marker for DI and scanning.")]
public sealed class AssemblyMarker
{

}