using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.SharedKernel.PagedResults;

/// <summary>
/// Represents pagination metadata with typed properties.
/// </summary>
public interface IPagedMetadata : IMetadata
{
    /// <summary>
    /// Gets the current page number. The first page is 1.
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Gets the requested number of items per page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    long TotalCount { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// Gets a value indicating whether another page is available.
    /// </summary>
    bool HasNextPage { get; }

    /// <summary>
    /// Gets a value indicating whether a previous page is available.
    /// </summary>
    bool HasPreviousPage { get; }

    /// <summary>
    /// Gets the number of items in the current page.
    /// </summary>
    int ItemCount { get; }
}

/// <summary>
/// Represents the result of an operation that returns a paged collection of values
/// and may contain one or more errors.
/// </summary>
/// <typeparam name="TValue">The type of item contained in the paged result.</typeparam>
/// <typeparam name="TError">The type of error contained in the result.</typeparam>
public interface IPagedResult<TValue, TError> 
    : IResult<IReadOnlyList<TValue>, TError>, IMetadata
    where TError : IError
{

    /// <summary>
    /// Gets the paged metadata. Never null.
    /// </summary>
    IPagedMetadata PagedMetadata { get; }
}
