using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;
using MotivPlanBackend.Application.Features.DataTransferObjects.Workout;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.Database;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Test.IntegrationTests;

public sealed class GetWorkoutByIdQueryHandlerTests
{

    [Fact]
    public async Task HandleShouldReturnWorkoutWithExercisesWhenWorkoutExists()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();

        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<GetWorkoutByIdQueryHandler>();
        using var context = scope.ServiceProvider.GetRequiredService<MotivPlanDbContext>();
        var _dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        await context.Database.EnsureCreatedAsync();

        var exercise1 = new ExerciseEntity
        {
            Id = 1,
            Title = "Push-up",
            Description = "Classic push-up",
            LastModified = _dateTimeProvider.UtcNow,
            ModifiedBy = "System"
        };

        var exercise2 = new ExerciseEntity
        {
            Id = 2,
            Title = "Plank",
            Description = "Core hold",
            LastModified = _dateTimeProvider.UtcNow,
            ModifiedBy = "System"
        };

        var workout = new WorkoutEntity
        {
            Id = 1,
            Title = "Daily workout",
            Schedule = DateOnly.FromDateTime(_dateTimeProvider.UtcNow),
            LastModified = _dateTimeProvider.UtcNow,
            ModifiedBy = "System"
        };

        var workoutExercise1 = new WorkoutExerciseEntity
        {
            WorkoutId = 1,
            ExerciseId = 1,
            Workout = workout,
            Exercise = exercise1,
            Sets = 3,
            Reps = 12,
            IsTime = false,
            Duration = null
        };

        var workoutExercise2 = new WorkoutExerciseEntity
        {
            WorkoutId = 1,
            ExerciseId = 2,
            Workout = workout,
            Exercise = exercise2,
            Sets = 3,
            Reps = null,
            IsTime = true,
            Duration = TimeSpan.FromSeconds(45)
        };

        context.Exercises.AddRange(exercise1, exercise2);
        context.Workouts.Add(workout);
        context.WorkoutsExercises.AddRange(workoutExercise1, workoutExercise2);

        await context.SaveChangesAsync();

        var query = new GetWorkoutByIdQuery(new WorkoutDataTransferObject(1));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Exercises.Should().HaveCountGreaterThan(0);
        result.Value.Title.Should().Be("Daily workout");
        result.Value.Exercises.Should().HaveCount(2);

        result.Value.Exercises.Should().Contain(x =>
            x.Id == 1 &&
            x.Title == "Push-up" &&
            x.Sets == 3 &&
            !x.IsTime &&
            x.Reps == 12);

        result.Value.Exercises.Should().Contain(x =>
            x.Id == 2 &&
            x.Title == "Plank" &&
            x.Sets == 3 &&
            x.IsTime &&
            x.Duration == TimeSpan.FromSeconds(45));
    }

    [Fact]
    public async Task HandleShouldReturnNotFoundWhenWorkoutDoesNotExist()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();
        using var scope = provider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<GetWorkoutByIdQueryHandler>();
        var query = new GetWorkoutByIdQuery(new WorkoutDataTransferObject(3));
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task HandleShouldReturnExercisesWithTimeBasedValuesWhenExerciseIsTimeBased()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();
        using var scope = provider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<GetWorkoutByIdQueryHandler>();
        var query = new GetWorkoutByIdQuery(new WorkoutDataTransferObject(1));
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Exercises.Should().Contain(x =>
            x.IsTime &&
            x.Duration.HasValue &&
            x.Reps == null);
    }

    [Fact]
    public async Task HandleShouldReturnExercisesWithRepBasedValuesWhenExerciseIsRepBased()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();
        using var scope = provider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<GetWorkoutByIdQueryHandler>();
        var query = new GetWorkoutByIdQuery(new WorkoutDataTransferObject(1));
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Exercises.Should().Contain(x =>
            !x.IsTime &&
            !x.Duration.HasValue &&
            x.Reps == 12);
    }

    [Fact]
    public async Task HandleShouldReturnFailureWhenWorkoutDoesNotExist()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();
        using var scope = provider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<GetWorkoutByIdQueryHandler>();
        var query = new GetWorkoutByIdQuery(new WorkoutDataTransferObject(999));
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Obj.Should().Be("workout");
    }
}
