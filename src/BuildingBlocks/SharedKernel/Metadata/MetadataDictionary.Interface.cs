namespace BuildingBlocks.SharedKernel.Metadata;

/// <summary>
/// Defines a generic way to support metadata capabilities.
/// </summary>
/// <seealso cref="IDictionary{TKey,TValue}" />
public interface IMetadataDictionary : IDictionary<string, object>
{
}