using System.ComponentModel;

namespace MotivPlanBackend.Domain.Enums;

public enum WorkoutStatus
{
    [Description("Not Started")]
    NotStarted = 0,
    [Description("In Progress")]
    InProgress = 1,
    [Description("Completed")]
    Completed = 2,
    [Description("Cancelled")]
    Cancelled = 3
}
