namespace BuildingBlocks.SharedKernel.Results;

/// <summary>
/// Validates error properties and error lists for Result types.
/// </summary>
public static class ResultValidator
{
    /// <summary>
    /// Validates that an error list is non-empty, does not exceed the maximum count,
    /// and all errors share the same HTTP status code.
    /// </summary>
    /// <typeparam name="TError">The type of error in the list.</typeparam>
    /// <param name="errors">The list of errors to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the list is empty, exceeds the maximum count, or contains mixed status codes.</exception>
    public static void ValidateErrors<TError>(
        IReadOnlyList<TError> errors, bool isFailure = true)
        where TError : IError
    {
        if (!isFailure)
            return;
        if (errors.Count == 0)
        {
            throw new ArgumentException(
                $"{ResultConstant.Codes.EmptyErrors} : {ResultConstant.Messages.EmptyErrors}",
                nameof(errors));
        }

        if (errors.Count > ResultConstant.Constraints.MaxErrors)
        {
            throw new ArgumentException(
                $"{ResultConstant.Codes.ExceedsMaxErrors} : {string.Format(ResultConstant.Messages.ExceedsMaxErrors, ResultConstant.Constraints.MaxErrors)}",
                nameof(errors));
        }

        var status = errors[0].Status;

        if (errors.Any(error => error.Status != status))
        {
            throw new ArgumentException(
                $"{ResultConstant.Codes.MixedStatusCodes} : {ResultConstant.Messages.MixedStatusCodes}",
                nameof(errors));
        }
    }
}
