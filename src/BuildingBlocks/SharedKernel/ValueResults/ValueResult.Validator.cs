namespace BuildingBlocks.SharedKernel.ValueResults;

/// <summary>
/// Validates properties and state for ValueResult types.
/// </summary>
public static class ValueResultValidator
{
    /// <summary>
    /// Validates that a success result has a non-null value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="isSuccess">Whether the result is a success.</param>
    /// <exception cref="ArgumentException">Thrown when a success result has a null value.</exception>
    public static void ValidateValueForSuccess<TValue>(
        TValue? value,
        bool isSuccess)
    {
        if (isSuccess && value is null)
        {
            throw new ArgumentException(
                $"{ValueResultConstant.Codes.ValueRequired} : {ValueResultConstant.Messages.ValueRequired}",
                nameof(value));
        }
    }

    /// <summary>
    /// Validates that a failure result does not have a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="isSuccess">Whether the result is a success.</param>
    /// <exception cref="ArgumentException">Thrown when a failure result has a non-null value.</exception>
    public static void ValidateValueForFailure<TValue>(
        TValue? value,
        bool isSuccess)
    {
        if (!isSuccess && value is not null)
        {
            throw new ArgumentException(
                $"{ValueResultConstant.Codes.ValueNotAllowed} : {ValueResultConstant.Messages.ValueNotAllowed}",
                nameof(value));
        }
    }

    /// <summary>
    /// Validates that the status code is within the valid HTTP range.
    /// </summary>
    /// <param name="status">The status code to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the status is not within the HTTP range.</exception>
    public static void ValidateStatusCode(int status)
    {
        if (status < 100 || status > 599)
        {
            throw new ArgumentOutOfRangeException(
                nameof(status),
                $"{ValueResultConstant.Codes.InvalidStatusCode} : {string.Format(ValueResultConstant.Messages.InvalidStatusCode, status)}");
        }
    }

    /// <summary>
    /// Validates that a success result has a 2xx status code.
    /// </summary>
    /// <param name="status">The status code to validate.</param>
    /// <param name="isSuccess">Whether the result is a success.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a success result does not have a 2xx status code.</exception>
    public static void ValidateSuccessStatus(
        int status,
        bool isSuccess)
    {
        if (isSuccess && (status < 200 || status > 299))
        {
            throw new ArgumentOutOfRangeException(
                nameof(status),
                $"{ValueResultConstant.Codes.InvalidSuccessStatus} : {string.Format(ValueResultConstant.Messages.InvalidSuccessStatus, status)}");
        }
    }

    /// <summary>
    /// Validates that a failure result has a 4xx or 5xx status code.
    /// </summary>
    /// <param name="status">The status code to validate.</param>
    /// <param name="isSuccess">Whether the result is a success.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a failure result does not have a 4xx/5xx status code.</exception>
    public static void ValidateFailureStatus(
        int status,
        bool isSuccess)
    {
        if (!isSuccess && (status < 400 || status > 599))
        {
            throw new ArgumentOutOfRangeException(
                nameof(status),
                $"{ValueResultConstant.Codes.InvalidFailureStatus} : {string.Format(ValueResultConstant.Messages.InvalidFailureStatus, status)}");
        }
    }

    /// <summary>
    /// Validates a complete ValueResult state.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="isSuccess">Whether the result is a success.</param>
    /// <param name="status">The HTTP status code.</param>
    public static void ValidateResult<TValue>(
        TValue? value,
        bool isSuccess,
        int status)
    {
        ValidateStatusCode(status);
        ValidateValueForSuccess(value, isSuccess);
    }
}
