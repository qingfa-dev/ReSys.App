namespace BuildingBlocks.SharedKernel.Primitives.Entities;

/// <summary>
/// Defines a generic identity typically associated with a storage such as a database.
/// </summary>
/// <typeparam name="TKey">The type of the identifier.</typeparam>
public interface IIdentity<TKey>
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    /// <value>The value of the identifier.</value>
    TKey Id { get; }
}

/// <summary>
/// Defines an Entity as specified in Domain Driven Design.
/// </summary>
/// <typeparam name="TKey">The type of the identifier.</typeparam>
/// <seealso cref="IIdentity{TKey}" />
public interface IEntity<TKey> : IIdentity<TKey>
{
}