using MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;
using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Application.Features.DataTransferObjects.Workout;

public record WorkoutExerciseDataTransferObject(
    int Id,
    string Title,
    DateOnly Schedule,
    WorkoutStatus WorkoutStatus,
    ICollection<ExerciseDataTransferObject> Exercises);