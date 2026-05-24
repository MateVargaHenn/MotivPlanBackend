using Microsoft.AspNetCore.Identity;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Shared.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotivPlanBackend.Domain.Entities;

public class ProfileEntity : Entity
{
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }

    [ForeignKey(nameof(Entities.SexEntity))]
    public Sex SexId { get; set; }
    public SexEntity SexEntity { get; set; } = default!;
    public double Weight { get; set; }
    public int Height { get; set; }
    public ICollection<ProfilePreferenceEntity> ProfilePreferences { get; } = [];
}
