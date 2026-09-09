namespace BuildingBlocks.SharedKernel.Primitives.Entities;

/// <summary>
/// Represents a base entity with a typed identifier.
/// Provides equality based on the <see cref="IEntity{TKey}.Id"/> property.
/// </summary>
/// <typeparam name="TKey">The type of the identifier.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="Entity{TKey}"/> class.
/// </remarks>
/// <param name="id">The unique identifier.</param>
public abstract class Entity<TKey>(TKey id) : IEntity<TKey>, IEquatable<Entity<TKey>>
    where TKey : notnull
{

    /// <inheritdoc />
    public TKey Id { get; } = id;

    /// <summary>
    /// Determines whether two entities are equal based on their identifiers.
    /// </summary>
    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two entities are not equal based on their identifiers.
    /// </summary>
    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public bool Equals(Entity<TKey>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return GetType() == other.GetType() && Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TKey>);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}