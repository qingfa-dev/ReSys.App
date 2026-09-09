namespace BuildingBlocks.SharedKernel.PagedResults;

public sealed partial class PagedMetadata
{
    public PagedMetadata WithKey(string key, object? value)
    {
        var newKeyValues = new Dictionary<string, object?>(_keyValues, StringComparer.OrdinalIgnoreCase)
        {
            [key] = value
        };

        return new PagedMetadata(
            PageNumber,
            PageSize,
            TotalCount,
            TotalPages,
            HasNextPage,
            HasPreviousPage,
            ItemCount,
            newKeyValues);
    }

    public PagedMetadata WithPage(int pageNumber)
    {
        var newKeyValues = new Dictionary<string, object?>(_keyValues, StringComparer.OrdinalIgnoreCase)
        {
            [KeyPageNumber] = pageNumber
        };

        return new PagedMetadata(
            pageNumber, PageSize, TotalCount, TotalPages, HasNextPage, HasPreviousPage, ItemCount, newKeyValues);
    }

    public PagedMetadata WithPageSize(int pageSize)
    {
        var newKeyValues = new Dictionary<string, object?>(_keyValues, StringComparer.OrdinalIgnoreCase)
        {
            [KeyPageSize] = pageSize
        };

        return new PagedMetadata(
            PageNumber, pageSize, TotalCount, TotalPages, HasNextPage, HasPreviousPage, ItemCount, newKeyValues);
    }
}