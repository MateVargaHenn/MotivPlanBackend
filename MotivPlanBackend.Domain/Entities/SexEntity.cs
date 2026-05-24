using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Domain.Entities;

public class SexEntity : EnumEntity<Sex>
{
    public ICollection<ProfileEntity> Profiles { get; } = [];
}
