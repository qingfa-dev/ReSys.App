namespace BuildingBlocks.SharedKernel.PagedResults;

public partial record PagedResult<TValue, TError> : Result<IReadOnlyList<TValue>, TError>, IPagedResult<TValue, TError>
    where TError : IError
{
    #region Fluent Methods

    /// <summary>
    /// Sets the paged metadata. Returns a new instance with updated metadata.
    /// </summary>
    /// <param name="pagedMetadata">The paged metadata to set.</param>
    /// <returns>A new <see cref="PagedResult{TValue, TError}"/> with updated metadata.</returns>
    public PagedResult<TValue, TError> WithPagedMetadata(PagedMetadata pagedMetadata)
    {
        return new PagedResult<TValue, TError>(
            Value,
            IsSuccess,
            Status,
            Errors,
            pagedMetadata);
    }

    /// <summary>
    /// Updates the page number. Returns a new instance.
    /// </summary>
    public PagedResult<TValue, TError> WithPage(int pageNumber)
    {
        var newMetadata = PagedMetadata.WithPage(pageNumber);
        return WithPagedMetadata(newMetadata);
    }

    /// <summary>
    /// Updates the page size. Returns a new instance.
    /// </summary>
    public PagedResult<TValue, TError> WithPageSize(int pageSize)
    {
        var newMetadata = PagedMetadata.WithPageSize(pageSize);
        return WithPagedMetadata(newMetadata);
    }

    /// <summary>
    /// Adds or updates a custom key-value pair in the paged metadata.
    /// The key is also added to the base Result Metadata dictionary.
    /// </summary>
    public PagedResult<TValue, TError> WithKey(string key, object? value)
    {
        var newPagedMetadata = PagedMetadata.WithKey(key, value);

        return new PagedResult<TValue, TError>(
            Value,
            IsSuccess,
            Status,
            Errors,
            newPagedMetadata);
    }

    /// <summary>
    /// Moves to the next page. Returns a new instance clamped to the last page.
    /// </summary>
    public PagedResult<TValue, TError> NextPage()
    {
        if (!PagedMetadata.HasNextPage)
        {
            return this;
        }

        var nextPage = PagedMetadata.PageNumber + 1;
        var newMetadata = PagedMetadata.WithPage(nextPage);
        return WithPagedMetadata(newMetadata);
    }

    /// <summary>
    /// Moves to the previous page. Returns a new instance clamped to page 1.
    /// </summary>
    public PagedResult<TValue, TError> PreviousPage()
    {
        if (!PagedMetadata.HasPreviousPage)
        {
            return this;
        }

        var prevPage = PagedMetadata.PageNumber - 1;
        var newMetadata = PagedMetadata.WithPage(prevPage);
        return WithPagedMetadata(newMetadata);
    }

    /// <summary>
    /// Moves to the first page. Returns a new instance.
    /// </summary>
    public PagedResult<TValue, TError> FirstPage()
    {
        var newMetadata = PagedMetadata.WithPage(1);
        return WithPagedMetadata(newMetadata);
    }

    /// <summary>
    /// Moves to the last page. Returns a new instance.
    /// </summary>
    public PagedResult<TValue, TError> LastPage()
    {
        var newMetadata = PagedMetadata.WithPage(PagedMetadata.TotalPages);
        return WithPagedMetadata(newMetadata);
    }

    #endregion
}
