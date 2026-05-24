using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;
using MotivPlanBackend.Persistence.BaseOptions.Context.EnumEntityModelBuilders;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Persistence.Database;

public sealed class MotivPlanDbContext(DbContextOptions<MotivPlanDbContext> options,
    IDomainEventsDispatcher? domainEventsDispatcher)
        : IdentityDbContext<IdentityUser, IdentityRole, string>(options), IMotivPlanDbContext
{
    private readonly IDomainEventsDispatcher? _domainEventsDispatcher = domainEventsDispatcher;

    public DbSet<ExerciseEntity> Exercises { get; set; }
    public DbSet<PreferenceEntity> Preferences { get; set; }
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<UserWorkoutEntity> UserWorkout { get; set; }
    public DbSet<WorkoutEntity> Workouts { get; set; }
    public DbSet<WorkoutExerciseEntity> WorkoutsExercises { get; set; }
    public DbSet<WorkoutStatusEntity> WorkoutStatus { get; set; }
    public DbSet<SexEntity> Sex { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Ensure.NotNull(builder);
        base.OnModelCreating(builder);

        // Base configuration
        builder
            .HasDefaultSchema(Schemas.Default)
            .UseCollation("utf8_general_ci");
        builder?.ApplyConfigurationsFromAssembly(typeof(MotivPlanDbContext).Assembly);

        // Existing entity model builders
        new ExerciseEntityModelBuilder().ConfigureModel(builder!);
        new WorkoutEntityModelBuilder().ConfigureModel(builder!);
        new WorkoutExerciseEntityModelBuilder().ConfigureModel(builder!);
        new WorkoutStatusEntityModelBuilder().ConfigureModel(builder!);
        new ProfileEntityModelBuilder().ConfigureModel(builder!);
        new PreferenceEntityModelBuilder().ConfigureModel(builder!);
        new SexEntityModelBuilder().ConfigureModel(builder!);
        new UserWorkoutEntityModelBuilder().ConfigureModel(builder!);

        // Identity model builders
        new IdentityModelBuilder().ConfigureModel(builder!);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail
        await PublishDomainEventsAsync();

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                ICollection<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();
        if (domainEvents.Count == 0 || _domainEventsDispatcher is null)
        {
            return;
        }

        await _domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
