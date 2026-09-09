namespace BuildingBlocks.SharedKernel.Primitives.StronglyTypedIds;

/// <summary>
/// Represents a strongly-typed identifier that wraps a primitive value.
/// Provides type safety to prevent mixing identifiers of different entities.
/// </summary>
/// <typeparam name="TValue">The underlying type of the identifier value.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="StronglyTypedId{TValue}"/> class.
/// </remarks>
/// <param name="value">The identifier value.</param>
public abstract class StronglyTypedId<TValue>(TValue value) : IEquatable<StronglyTypedId<TValue>>
    where TValue : IEquatable<TValue>
{

    /// <summary>
    /// Gets the underlying value of the identifier.
    /// </summary>
    public TValue Value { get; } = value;

    /// <summary>
    /// Determines whether two strongly-typed IDs are equal.
    /// </summary>
    public static bool operator ==(StronglyTypedId<TValue>? left, StronglyTypedId<TValue>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two strongly-typed IDs are not equal.
    /// </summary>
    public static bool operator !=(StronglyTypedId<TValue>? left, StronglyTypedId<TValue>? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public bool Equals(StronglyTypedId<TValue>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return GetType() == other.GetType() && Value.Equals(other.Value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return Equals(obj as StronglyTypedId<TValue>);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value.ToString() ?? string.Empty;
    }
}
