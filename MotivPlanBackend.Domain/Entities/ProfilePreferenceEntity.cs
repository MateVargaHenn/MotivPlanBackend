using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Domain.Entities;

public class ProfilePreferenceEntity
{
    public int ProfileId { get; set; }
    public ProfileEntity Profile { get; set; } = null!;

    public Preference PreferenceId { get; set; }
    public PreferenceEntity Preference { get; set; } = null!;
}
