using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;

namespace MotivPlanBackend.Application.Features.Workout;


public sealed record SetWorkoutStatusCommand(WorkoutStatusDataTransferObject WorkoutStatusDto)
    : ICommand;