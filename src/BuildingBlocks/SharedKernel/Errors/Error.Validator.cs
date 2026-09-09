namespace BuildingBlocks.SharedKernel.Errors;

/// <summary>
/// Validates each field of the <see cref="IError"/> interface for the <see cref="Error"/> type.
/// </summary>
public static class ErrorValidator
{
    /// <summary>
    /// Validates that the status code is a valid HTTP status code.
    /// </summary>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the status is not within the HTTP range.</exception>
    public static void ValidateStatus(int status)
    {
        if (status < 100 || status > 599)
        {
            throw new ArgumentOutOfRangeException(
                nameof(status),
                $"{ErrorConstant.Codes.InvalidStatus} : {string.Format(ErrorConstant.Messages.InvalidStatus, status)}");
        }
    }

    /// <summary>
    /// Validates that the type is not null.
    /// </summary>
    /// <param name="type">The error type to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    public static void ValidateType(string type)
    {
        ArgumentNullException.ThrowIfNull(type);
    }

    /// <summary>
    /// Validates that an error code is not null, whitespace, and does not exceed the maximum length.
    /// </summary>
    /// <param name="code">The error code to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the code is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the code exceeds the maximum length.</exception>
    public static void ValidateCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        if (code.Length > ErrorConstant.Constraints.MaxCodeLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(code),
                $"{ErrorConstant.Codes.CodeMaxLength} : {string.Format(ErrorConstant.Messages.CodeMaxLength, ErrorConstant.Constraints.MaxCodeLength)}");
        }
    }

    /// <summary>
    /// Validates that an error description is not null, whitespace, and does not exceed the maximum length.
    /// </summary>
    /// <param name="description">The error description to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the description is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the description exceeds the maximum length.</exception>
    public static void ValidateDescription(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (description.Length > ErrorConstant.Constraints.MaxDescriptionLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(description),
                $"{ErrorConstant.Codes.DescriptionMaxLength} : {string.Format(ErrorConstant.Messages.DescriptionMaxLength, ErrorConstant.Constraints.MaxDescriptionLength)}");
        }
    }
}