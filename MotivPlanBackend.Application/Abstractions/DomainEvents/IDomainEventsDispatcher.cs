using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.Abstractions.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
