using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.Behaviors;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationDependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            //.AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            //    .AsImplementedInterfaces()
            //    .WithScopedLifetime()
            //.AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            //    .AsImplementedInterfaces()
            //    .WithScopedLifetime());

        //services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        //services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        //services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        //services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));

        services.Scan(scan => scan.FromAssembliesOf(typeof(ApplicationDependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        string licenseKey = Environment.GetEnvironmentVariable("MEDIATR_LICENSE_KEY")!;
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