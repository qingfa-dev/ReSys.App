namespace BuildingBlocks.SharedKernel.ValueResults;

public partial record Result<TValue, TError> : Result<TError>, IResult<TValue, TError>
    where TError : IError
{
    #region Implicit Operators

    /// <summary>
    /// Implicitly converts a value to a success <see cref="Result{TValue, TError}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<TValue, TError>(TValue value)
    {
        return Success(value);
    }

    /// <summary>
    /// Implicitly converts an error to a failure <see cref="Result{TValue, TError}"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result<TValue, TError>(TError error)
    {
        return Failure(error);
    }

    /// <summary>
    /// Implicitly converts a list of errors to a failure <see cref="Result{TValue, TError}"/>.
    /// </summary>
    /// <param name="errors">The list of errors to convert.</param>
    public static implicit operator Result<TValue, TError>(List<TError> errors)
    {
        return Failure(errors);
    }

    /// <summary>
    /// Implicitly converts an array of errors to a failure <see cref="Result{TValue, TError}"/>.
    /// </summary>
    /// <param name="errors">The array of errors to convert.</param>
    public static implicit operator Result<TValue, TError>(TError[] errors)
    {
        return Failure(errors);
    }

    #endregion
}
