using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Test.Dispatchers;

internal sealed class TestDomainEventsDispatcher : IDomainEventsDispatcher
{
    public Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}