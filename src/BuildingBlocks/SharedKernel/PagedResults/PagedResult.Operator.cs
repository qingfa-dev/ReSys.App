namespace BuildingBlocks.SharedKernel.PagedResults;

public partial record PagedResult<TValue, TError> : Result<IReadOnlyList<TValue>, TError>, IPagedResult<TValue, TError>
    where TError : IError
{
    #region Implicit Operators

    /// <summary>
    /// Implicitly converts a list of items to a success <see cref="PagedResult{TValue, TError}"/>.
    /// Uses default paging: page 1, pageSize = items.Count, totalCount = items.Count.
    /// </summary>
    public static implicit operator PagedResult<TValue, TError>(List<TValue> items)
    {
        return Success(items.AsReadOnly(), 1, items.Count, items.Count);
    }

    /// <summary>
    /// Implicitly converts an array of items to a success <see cref="PagedResult{TValue, TError}"/>.
    /// Uses default paging: page 1, pageSize = items.Length, totalCount = items.Length.
    /// </summary>
    public static implicit operator PagedResult<TValue, TError>(TValue[] items)
    {
        return Success(items.ToList().AsReadOnly(), 1, items.Length, items.Length);
    }

    /// <summary>
    /// Implicitly converts an error to a failure <see cref="PagedResult{TValue, TError}"/>.
    /// </summary>
    public static implicit operator PagedResult<TValue, TError>(TError error)
    {
        return Failure(error);
    }

    /// <summary>
    /// Implicitly converts a list of errors to a failure <see cref="PagedResult{TValue, TError}"/>.
    /// </summary>
    public static implicit operator PagedResult<TValue, TError>(List<TError> errors)
    {
        return Failure(errors.ToArray());
    }

    /// <summary>
    /// Implicitly converts an array of errors to a failure <see cref="PagedResult{TValue, TError}"/>.
    /// </summary>
    public static implicit operator PagedResult<TValue, TError>(TError[] errors)
    {
        return Failure(errors);
    }

    #endregion
}