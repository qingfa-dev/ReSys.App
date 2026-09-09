namespace BuildingBlocks.SharedKernel.Metadata;


/// <summary>
/// Defines a generic way to associate metadata with any type of object.
/// </summary>
/// <remarks>
/// Implement this capability on aggregate, request, or message base types that
/// need metadata. Use <see cref="MetadataExtensions"/> for common read, write,
/// and merge operations; the interface intentionally only exposes storage.
/// </remarks>
public interface IMetadata
{
    /// <summary>
    /// Gets the associated metadata of this instance.
    /// </summary>
    /// <value>The associated metadata of this instance.</value>
    public IMetadataDictionary Metadata { get; }
}