using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.SharedKernel.ValueResults;

public partial record Result<TValue, TError> : Result<TError>, IResult<TValue, TError>
    where TError : IError
{
    #region Create

    /// <summary>
    /// Creates a <see cref="Result{TValue, TError}"/> with the specified parameters.
    /// This is the low-level factory method that replaces the protected constructor.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <param name="errors">The list of errors.</param>
    /// <returns>A new <see cref="Result{TValue, TError}"/>.</returns>
    public static Result<TValue, TError> Create(
        TValue? value,
        bool isSuccess,
        int status,
        IReadOnlyList<TError>? errors = null)
    {
        ValueResultValidator.ValidateResult(value, isSuccess, status);

        var errorList = errors is null ? [] : errors.ToList();

        return new Result<TValue, TError>(
            value,
            isSuccess,
            status,
            errorList);
    }

    #endregion

    #region Success Factories

    /// <summary>
    /// Creates a success result with the specified value and 200 OK status.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/> with status 200.</returns>
    public static Result<TValue, TError> Success(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Create(
            value: value,
            isSuccess: true,
            status: StatusCodes.Status200OK);
    }

    /// <summary>
    /// Creates a success result with the specified value and a custom HTTP status code.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <param name="status">The custom HTTP status code.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/>.</returns>
    public static Result<TValue, TError> Success(TValue value, int status)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Create(
            value: value,
            isSuccess: true,
            status: status);
    }

    /// <summary>
    /// Creates a 200 OK success result with the specified value.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/> with status 200.</returns>
    public static Result<TValue, TError> Ok(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Create(
            value: value,
            isSuccess: true,
            status: StatusCodes.Status200OK);
    }

    /// <summary>
    /// Creates a 201 Created success result with the specified value.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/> with status 201.</returns>
    public static Result<TValue, TError> Created(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Create(
            value: value,
            isSuccess: true,
            status: StatusCodes.Status201Created);
    }

    /// <summary>
    /// Creates a 202 Accepted success result with the specified value.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/> with status 202.</returns>
    public static Result<TValue, TError> Accepted(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Create(
            value: value,
            isSuccess: true,
            status: StatusCodes.Status202Accepted);
    }

    /// <summary>
    /// Creates a 204 No Content success result with the specified value.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/> with status 204.</returns>
    public static Result<TValue, TError> NoContent(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Create(
            value: value,
            isSuccess: true,
            status: StatusCodes.Status204NoContent);
    }

    #endregion

    #region Failure Factories

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    /// <returns>A failure <see cref="Result{TValue, TError}"/>.</returns>
    public static new Result<TValue, TError> Failure(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return Create(
            value: default,
            isSuccess: false,
            status: error.Status,
            errors: [error]);
    }

    /// <summary>
    /// Creates a failure result with the specified errors.
    /// </summary>
    /// <param name="errors">The errors describing the failure.</param>
    /// <returns>A failure <see cref="Result{TValue, TError}"/>.</returns>
    public static new Result<TValue, TError> Failure(params TError[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToList();

        ResultValidator.ValidateErrors(errorList);

        return Create(
            value: default,
            isSuccess: false,
            status: errorList[0].Status,
            errors: errorList);
    }

    /// <summary>
    /// Creates a failure result with the specified errors.
    /// </summary>
    /// <param name="errors">The errors describing the failure.</param>
    /// <returns>A failure <see cref="Result{TValue, TError}"/>.</returns>
    public static new Result<TValue, TError> Failure(IEnumerable<TError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToList();

        ResultValidator.ValidateErrors(errorList);

        return Create(
            value: default,
            isSuccess: false,
            status: errorList[0].Status,
            errors: errorList);
    }

    /// <summary>
    /// Creates a result from the specified value.
    /// </summary>
    /// <param name="value">The value to wrap in a success result.</param>
    /// <returns>A success <see cref="Result{TValue, TError}"/>.</returns>
    public static Result<TValue, TError> From(TValue value)
    {
        return Success(value);
    }

    /// <summary>
    /// Creates a result from the specified error.
    /// </summary>
    /// <param name="error">The error to wrap in a failure result.</param>
    /// <returns>A failure <see cref="Result{TValue, TError}"/>.</returns>
    public static Result<TValue, TError> From(TError error)
    {
        return Failure(error);
    }

    #endregion
}