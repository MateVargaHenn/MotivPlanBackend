using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Application.Abstractions.DomainEvents;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Persistence.BaseOptions.Context.EntityModelBuilders;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Persistence.Database;

public sealed class MotivPlanDbContext
    : DbContext, IMotivPlanDbContext
{
    private readonly IDomainEventsDispatcher? _domainEventsDispatcher;

    public DbSet<WorkoutEntity> Workouts { get; set; }
    public DbSet<ExerciseEntity> Exercises { get; set; }
    public DbSet<WorkoutExerciseEntity> WorkoutsExercises { get; set; }

    public MotivPlanDbContext(DbContextOptions<MotivPlanDbContext> options,
        IDomainEventsDispatcher? domainEventsDispatcher)
        : base(options) =>
            _domainEventsDispatcher = domainEventsDispatcher;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Ensure.NotNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        // Base configuration
        modelBuilder
            .HasDefaultSchema(Schemas.Default)
            .UseCollation("utf8_general_ci");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotivPlanDbContext).Assembly);

        // Existing entity model builders
        new ExerciseEntityModelBuilder().ConfigureModel(modelBuilder);
        new WorkoutEntityModelBuilder().ConfigureModel(modelBuilder);
        new WorkoutExerciseEntityModelBuilder().ConfigureModel(modelBuilder);
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
