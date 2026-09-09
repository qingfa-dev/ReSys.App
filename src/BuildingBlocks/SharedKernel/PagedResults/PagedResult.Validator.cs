namespace BuildingBlocks.SharedKernel.PagedResults;

/// <summary>
/// Validates pagination parameters for PagedResult types.
/// </summary>
public static class PagedResultValidator
{
    /// <summary>
    /// Validates pagination parameters.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="totalCount">The total count.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when pagination parameters are invalid.</exception>
    public static void ValidatePaging(
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        if (pageNumber < PagedResultConstant.Constraints.MinPageNumber)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pageNumber),
                $"{PagedResultConstant.Codes.InvalidPageNumber} : {string.Format(PagedResultConstant.Messages.InvalidPageNumber, pageNumber)}");
        }

        if (pageSize < PagedResultConstant.Constraints.MinPageSize
            || pageSize > PagedResultConstant.Constraints.MaxPageSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pageSize),
                $"{PagedResultConstant.Codes.InvalidPageSize} : {string.Format(PagedResultConstant.Messages.InvalidPageSize, PagedResultConstant.Constraints.MinPageSize, PagedResultConstant.Constraints.MaxPageSize, pageSize)}");
        }

        if (totalCount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalCount),
                $"{PagedResultConstant.Codes.InvalidTotalCount} : {string.Format(PagedResultConstant.Messages.InvalidTotalCount, totalCount)}");
        }
    }
}