using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.SharedKernel.PagedResults;

public partial record PagedResult<TValue, TError> : Result<IReadOnlyList<TValue>, TError>, IPagedResult<TValue, TError>
    where TError : IError
{
    #region Create

    /// <summary>
    /// Creates a <see cref="PagedResult{TValue, TError}"/> with the specified parameters.
    /// </summary>
    public static PagedResult<TValue, TError> Create(
        IReadOnlyList<TValue> items,
        bool isSuccess,
        int status,
        List<TError>? errors = null,
        int pageNumber = 1,
        int pageSize = 10,
        long totalCount = 0)
    {
        PagedResultValidator.ValidatePaging(pageNumber, pageSize, totalCount);
        ValueResultValidator.ValidateResult(items, isSuccess, status);

        var pagedMetadata = PagedMetadata.From(pageNumber, pageSize, totalCount);

        var errorList = errors is null ? [] : errors.ToList();

        return new PagedResult<TValue, TError>(
            items,
            isSuccess,
            status,
            errorList,
            pagedMetadata);
    }

    #endregion

    #region Success Factories

    /// <summary>
    /// Creates a 200 OK success result with paged data.
    /// </summary>
    public static PagedResult<TValue, TError> Success(
        IReadOnlyList<TValue> items,
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        return Create(
            items: items,
            isSuccess: true,
            status: StatusCodes.Status200OK,
            errors: [],
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    /// <summary>
    /// Creates a 200 OK success result with paged data and custom status.
    /// </summary>
    public static PagedResult<TValue, TError> Success(
        IReadOnlyList<TValue> items,
        int pageNumber,
        int pageSize,
        long totalCount,
        int status)
    {
        return Create(
            items: items,
            isSuccess: true,
            status: status,
            errors: [],
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    /// <summary>
    /// Creates a 200 OK success result.
    /// </summary>
    public static PagedResult<TValue, TError> Ok(
        IReadOnlyList<TValue> items,
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        return Create(
            items: items,
            isSuccess: true,
            status: StatusCodes.Status200OK,
            errors: [],
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    /// <summary>
    /// Creates a 201 Created success result.
    /// </summary>
    public static PagedResult<TValue, TError> Created(
        IReadOnlyList<TValue> items,
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        return Create(
            items: items,
            isSuccess: true,
            status: StatusCodes.Status201Created,
            errors: [],
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    /// <summary>
    /// Creates a 202 Accepted success result.
    /// </summary>
    public static PagedResult<TValue, TError> Accepted(
        IReadOnlyList<TValue> items,
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        return Create(
            items: items,
            isSuccess: true,
            status: StatusCodes.Status202Accepted,
            errors: [],
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    #endregion

    #region Failure Factories

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    public static PagedResult<TValue, TError> Failure(
        TError error,
        int pageNumber = 1,
        int pageSize = 10,
        long totalCount = 0)
    {
        return Create(
            items: [],
            isSuccess: false,
            status: error.Status,
            errors: [error],
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    /// <summary>
    /// Creates a failure result with the specified errors.
    /// </summary>
    public static new PagedResult<TValue, TError> Failure(
        params TError[] errors)
    {
        var errorList = errors.ToList();
        return Create(
            items: [],
            isSuccess: false,
            status: errorList[0].Status,
            errors: errorList,
            pageNumber: 1,
            pageSize: 10,
            totalCount: 0);
    }

    /// <summary>
    /// Creates a failure result with the specified errors and paging.
    /// </summary>
    public static PagedResult<TValue, TError> Failure(
        int pageNumber,
        int pageSize,
        long totalCount,
        params TError[] errors)
    {
        var errorList = errors.ToList();
        return Create(
            items: [],
            isSuccess: false,
            status: errorList[0].Status,
            errors: errorList,
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalCount: totalCount);
    }

    #endregion
}
