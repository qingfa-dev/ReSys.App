namespace BuildingBlocks.SharedKernel.PagedResults;

public sealed partial class PagedMetadata
{
    public static PagedMetadata From(
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        var totalPages = pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 0;
        var hasNextPage = pageNumber < totalPages;
        var hasPreviousPage = pageNumber > 1;
        var count = 0;

        if (totalCount > 0 && pageSize > 0)
        {
            var firstItem = (pageNumber - 1) * pageSize + 1;
            var lastItem = Math.Min(firstItem + pageSize - 1, totalCount);
            count = (int)(lastItem - firstItem + 1);
        }

        return new PagedMetadata(
            pageNumber,
            pageSize,
            totalCount,
            totalPages,
            hasNextPage,
            hasPreviousPage,
            count);
    }
}