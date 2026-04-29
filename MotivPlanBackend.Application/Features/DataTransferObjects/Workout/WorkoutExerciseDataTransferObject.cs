using MotivPlanBackend.Application.Features.DataTransferObjects.Exercise;

namespace MotivPlanBackend.Application.Features.DataTransferObjects.Workout;

public record WorkoutExerciseDataTransferObject(
    int Id,
    string Title,
    DateOnly Schedule,
    List<ExerciseDataTransferObject> Exercises);