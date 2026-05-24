using Microsoft.AspNetCore.Identity;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Domain.Entities;

public class PreferenceEntity : EnumEntity<Preference>
{
    public ICollection<ProfilePreferenceEntity> ProfilePreferences { get; } = [];
}