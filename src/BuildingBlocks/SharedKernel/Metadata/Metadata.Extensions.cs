namespace BuildingBlocks.SharedKernel.Metadata;

/// <summary>
/// Provides common operations for metadata-bearing objects.
/// </summary>
public static class MetadataExtensions
{
    public static object? GetMetadata<T>(this T source, string key)
        where T : IMetadata
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return source.Metadata.TryGetValue(key, out var value) ? value : null;
    }

    public static T SetMetadata<T>(this T source, string key, object value)
        where T : IMetadata
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        source.Metadata[key] = value;
        return source;
    }

    public static TDestination MergeMetadata<TSource, TDestination>(
        this TDestination destination,
        TSource source)
        where TSource : IMetadata
        where TDestination : IMetadata
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(source);

        foreach (var entry in source.Metadata)
        {
            destination.Metadata.TryAdd(entry.Key, entry.Value);
        }

        return destination;
    }
}