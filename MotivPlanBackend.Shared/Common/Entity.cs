using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotivPlanBackend.Shared.Common;

public abstract class Entity : IBaseModificationDataEntity
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public DateTime LastModified { get; set; }
    public string ModifiedBy { get; set; } = null!;

    private readonly List<IDomainEvent> _domainEvents = [];

    public ICollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
