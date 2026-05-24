using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;
using MotivPlanBackend.Application.Features.Workout;
using MotivPlanBackend.Domain.Enums;
using MotivPlanBackend.Persistence.Database;

namespace MotivPlanBackend.Test.IntegrationTests;

public sealed class SetWorkoutStatusCommandHandlerTest
{
    [Fact]
    public async Task HandleShouldReturnWorkoutWithExercisesWhenWorkoutStatusIsSet()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();

        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<SetWorkoutStatusCommandHandler>();
        using var context = scope.ServiceProvider.GetRequiredService<MotivPlanDbContext>();
        await context.Database.EnsureCreatedAsync();
        // Act
        var command = new SetWorkoutStatusCommand(new WorkoutStatusDataTransferObject(1, WorkoutStatus.InProgress));
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task HandleShouldReturnWorkoutWithExercisesWhenResultIsFailure()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();

        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<SetWorkoutStatusCommandHandler>();
        using var context = scope.ServiceProvider.GetRequiredService<MotivPlanDbContext>();
        await context.Database.EnsureCreatedAsync();
        // Act
        var command = new SetWorkoutStatusCommand(new WorkoutStatusDataTransferObject(999, WorkoutStatus.InProgress));
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }
}
