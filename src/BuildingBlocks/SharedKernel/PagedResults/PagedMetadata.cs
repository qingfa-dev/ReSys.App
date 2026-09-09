using System.Text.Json.Serialization;

using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.SharedKernel.PagedResults;

/// <summary>
/// Represents pagination metadata. Implements both typed interface and flat key-value dictionary.
/// Never null. JSON serializes as flat key-value pairs.
/// </summary>
[JsonConverter(typeof(PagedMetadataConverter))]
public sealed partial class PagedMetadata : IPagedMetadata, IReadOnlyDictionary<string, object?>
{
    private readonly Dictionary<string, object?> _keyValues;
    private readonly MetadataDictionary _metadata;

    [JsonConstructor]
    private PagedMetadata(
        int pageNumber,
        int pageSize,
        long totalCount,
        int totalPages,
        bool hasNextPage,
        bool hasPreviousPage,
        int itemCount,
        Dictionary<string, object?> keyValues)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
        ItemCount = itemCount;
        _keyValues = keyValues;
        _metadata = CreateMetadata(keyValues);
    }

    public PagedMetadata(
        int pageNumber,
        int pageSize,
        long totalCount,
        int totalPages,
        bool hasNextPage,
        bool hasPreviousPage,
        int count)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
        ItemCount = count;

        _keyValues = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            [KeyPageNumber] = pageNumber,
            [KeyPageSize] = pageSize,
            [KeyTotalCount] = totalCount
        };
        _metadata = CreateMetadata(_keyValues);
    }

    /// <summary>
    /// Gets the metadata associated with this paged result.
    /// </summary>
    [JsonIgnore]
    public IMetadataDictionary Metadata => _metadata;

    public int PageNumber { get; }

    public int PageSize { get; }

    public long TotalCount { get; }

    public int TotalPages { get; }

    public bool HasNextPage { get; }

    public bool HasPreviousPage { get; }

    public int ItemCount { get; }

    private static MetadataDictionary CreateMetadata(
        IReadOnlyDictionary<string, object?> values)
    {
        var metadata = MetadataDictionary.Create();

        foreach (var (key, value) in values)
        {
            metadata[key] = value!;
        }

        return metadata;
    }
}