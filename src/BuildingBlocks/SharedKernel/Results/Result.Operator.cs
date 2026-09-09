namespace BuildingBlocks.SharedKernel.Results;

public partial record Result<TError> : IResult<TError>
{
    #region Implicit Operators

    /// <summary>
    /// Implicitly converts an error to a failure <see cref="Result{TError}"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result<TError>(TError error)
    {
        return Failure(error);
    }

    /// <summary>
    /// Implicitly converts a list of errors to a failure <see cref="Result{TError}"/>.
    /// </summary>
    /// <param name="errors">The list of errors to convert.</param>
    public static implicit operator Result<TError>(List<TError> errors)
    {
        return Failure(errors);
    }

    /// <summary>
    /// Implicitly converts an array of errors to a failure <see cref="Result{TError}"/>.
    /// </summary>
    /// <param name="errors">The array of errors to convert.</param>
    public static implicit operator Result<TError>(TError[] errors)
    {
        return Failure(errors);
    }

    #endregion
}
