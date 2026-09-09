using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.SharedKernel.Results;

public partial record Result<TError> : IResult<TError>
    where TError : IError
{
    #region Create

    /// <summary>
    /// Creates a <see cref="Result{TError}"/> with the specified parameters.
    /// This is the low-level factory method that replaces the protected constructor.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <param name="errors">The list of errors.</param>
    /// <returns>A new <see cref="Result{TError}"/>.</returns>
    public static Result<TError> Create(
        bool isSuccess,
        int status,
        IReadOnlyList<TError>? errors = null)
    {
        var errorList = errors is null ? [] : errors.ToList();

        return new Result<TError>(
            isSuccess,
            status,
            errorList);
    }

    #endregion

    #region Success Factories

    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <returns>A success <see cref="Result{TError}"/>.</returns>
    public static Result<TError> Success()
    {
        return Create(
            isSuccess: true,
            status: StatusCodes.Status200OK);
    }

    /// <summary>
    /// Creates a success result with a custom HTTP status code.
    /// </summary>
    /// <param name="status">The custom HTTP status code.</param>
    /// <returns>A success <see cref="Result{TError}"/>.</returns>
    public static Result<TError> Success(int status)
    {
        return Create(
            isSuccess: true,
            status: status);
    }

    /// <summary>
    /// Creates a 200 OK success result.
    /// </summary>
    /// <returns>A success <see cref="Result{TError}"/> with status 200.</returns>
    public static Result<TError> Ok()
    {
        return Create(
            isSuccess: true,
            status: StatusCodes.Status200OK);
    }

    /// <summary>
    /// Creates a 201 Created success result.
    /// </summary>
    /// <returns>A success <see cref="Result{TError}"/> with status 201.</returns>
    public static Result<TError> Created()
    {
        return Create(
            isSuccess: true,
            status: StatusCodes.Status201Created);
    }

    /// <summary>
    /// Creates a 202 Accepted success result.
    /// </summary>
    /// <returns>A success <see cref="Result{TError}"/> with status 202.</returns>
    public static Result<TError> Accepted()
    {
        return Create(
            isSuccess: true,
            status: StatusCodes.Status202Accepted);
    }

    /// <summary>
    /// Creates a 204 No Content success result.
    /// </summary>
    /// <returns>A success <see cref="Result{TError}"/> with status 204.</returns>
    public static Result<TError> NoContent()
    {
        return Create(
            isSuccess: true,
            status: StatusCodes.Status204NoContent);
    }

    #endregion

    #region Failure Factories

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    /// <returns>A failure <see cref="Result{TError}"/>.</returns>
    internal static Result<TError> Failure(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return Create(
            isSuccess: false,
            status: error.Status,
            errors: [error]);
    }

    /// <summary>
    /// Creates a failure result with the specified errors.
    /// </summary>
    /// <param name="errors">The errors describing the failure.</param>
    /// <returns>A failure <see cref="Result{TError}"/>.</returns>
    internal static Result<TError> Failure(params TError[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToList().AsReadOnly();

        ResultValidator.ValidateErrors(errorList, isFailure: true);

        return Create(
            isSuccess: false,
            status: errorList[0].Status,
            errors: errorList);
    }

    /// <summary>
    /// Creates a failure result with the specified errors.
    /// </summary>
    /// <param name="errors">The errors describing the failure.</param>
    /// <returns>A failure <see cref="Result{TError}"/>.</returns>
    public static Result<TError> Failure(params IEnumerable<TError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToList().AsReadOnly();

        ResultValidator.ValidateErrors(errorList, isFailure: true);

        return Create(
            isSuccess: false,
            status: errorList[0].Status,
            errors: errorList);
    }

    #endregion
}