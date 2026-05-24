namespace MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;

public record ExerciseDataTransferObject(
    int Id,
    string Title,
    string Description,
    int Sets,
    bool IsTime,
    int? Reps,
    TimeSpan? Duration
);
