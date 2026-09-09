namespace BuildingBlocks.SharedKernel.Primitives.Enumerations;

/// <summary>
/// Represents a type-safe enumeration as an alternative to enum.
/// Provides a base class for creating enumeration types with associated values and behavior.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Enumeration{TId, TValue}"/> class.
/// </remarks>
/// <param name="id">The identifier.</param>
/// <param name="value">The value.</param>
public abstract class Enumeration<TId, TValue>(TId id, TValue value) : IEquatable<Enumeration<TId, TValue>>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// Gets the identifier of the enumeration.
    /// </summary>
    public TId Id { get; } = id;

    /// <summary>
    /// Gets the display value of the enumeration.
    /// </summary>
    public TValue Value { get; } = value;

    /// <summary>
    /// Gets the display name of the enumeration.
    /// </summary>
    public string Name { get; } = value?.ToString() ?? string.Empty;

    /// <summary>
    /// Gets all available values of the enumeration.
    /// </summary>
    /// <returns>An enumerable of all enumeration values.</returns>
    protected static IEnumerable<T> GetAll<T>()
        where T : Enumeration<TId, TValue>
    {
        return typeof(T).GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    /// <summary>
    /// Finds an enumeration value by its identifier.
    /// Returns a Result with a NotFound error if the value is not found.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="id">The identifier to search for.</param>
    /// <returns>A Result containing the matching enumeration value or a NotFound error.</returns>
    public static Result<T, Error> FromValue<T>(TId id)
        where T : Enumeration<TId, TValue>
    {
        var enumeration = GetAll<T>().FirstOrDefault(item => item.Id.Equals(id));

        if (enumeration is null)
        {
            return Error.NotFound(
                "Enumeration.NotFound",
                $"No {typeof(T).Name} found with Id '{id}'.");
        }

        return enumeration;
    }

    /// <summary>
    /// Finds an enumeration value by its value.
    /// Returns a Result with a NotFound error if the value is not found.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="value">The value to search for.</param>
    /// <returns>A Result containing the matching enumeration value or a NotFound error.</returns>
    public static Result<T, Error> FromValue<T>(TValue value)
        where T : Enumeration<TId, TValue>
    {
        var enumeration = GetAll<T>().FirstOrDefault(item =>
            EqualityComparer<TValue>.Default.Equals(item.Value, value));

        if (enumeration is null)
        {
            return Error.NotFound(
                "Enumeration.NotFound",
                $"No {typeof(T).Name} found with Value '{value}'.");
        }

        return enumeration;
    }

    /// <inheritdoc />
    public bool Equals(Enumeration<TId, TValue>? other)
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
        return Equals(obj as Enumeration<TId, TValue>);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }
}