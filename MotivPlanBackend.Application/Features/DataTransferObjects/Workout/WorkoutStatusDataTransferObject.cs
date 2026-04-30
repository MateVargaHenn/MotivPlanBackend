using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Application.Features.DataTransferObjects.Workout;

public record WorkoutStatusDataTransferObject(int WorkoutId, WorkoutStatus Status);
