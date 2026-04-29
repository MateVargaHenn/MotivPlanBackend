namespace MotivPlanBackend.Shared.Common;

public abstract class EnumEntity<TEnum>
    where TEnum : Enum
{
    public TEnum Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NormalizedName { get; set; } = default!;

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public override string ToString() => Name;
}
