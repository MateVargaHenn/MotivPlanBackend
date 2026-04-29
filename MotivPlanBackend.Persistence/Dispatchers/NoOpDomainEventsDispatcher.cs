using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Persistence.Dispatchers;

public sealed class NoOpDomainEventsDispatcher : IDomainEventsDispatcher
{
    public Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}