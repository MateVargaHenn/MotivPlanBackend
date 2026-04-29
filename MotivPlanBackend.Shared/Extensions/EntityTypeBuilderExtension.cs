using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotivPlanBackend.Shared.Extensions;

public static class EntityTypeBuilderExtension
{
    public static TEntityTypeBuilder Tap<TEntityTypeBuilder>(this TEntityTypeBuilder builder, Action<TEntityTypeBuilder>? action = null)
        where TEntityTypeBuilder : EntityTypeBuilder
    {
        action?.Invoke(builder);
        return builder;
    }
}