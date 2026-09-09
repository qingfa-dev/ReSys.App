namespace BuildingBlocks.SharedKernel.Results;

public partial record Result<TError> : IResult<TError>
    where TError : IError
{
    #region Functional Methods

    /// <summary>
    /// Matches the result to one of two functions: one for success and one for failure.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match functions.</typeparam>
    /// <param name="onSuccess">The function to invoke when the result is a success.</param>
    /// <param name="onFailure">The function to invoke when the result is a failure.</param>
    /// <returns>The value produced by the invoked function.</returns>
    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<IReadOnlyList<TError>, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess
            ? onSuccess()
            : onFailure(Errors);
    }

    /// <summary>
    /// Executes the specified action if the result is a success.
    /// Returns the original result to enable fluent chaining.
    /// </summary>
    /// <param name="onSuccess">The action to execute on success.</param>
    /// <returns>The current <see cref="Result{TError}"/>.</returns>
    public Result<TError> Tap(Action onSuccess)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        if (IsSuccess)
        {
            onSuccess();
        }

        return this;
    }

    /// <summary>
    /// Executes the specified action if the result is a failure, passing the errors list.
    /// Returns the original result to enable fluent chaining.
    /// </summary>
    /// <param name="onErrors">The action to execute with the errors list.</param>
    /// <returns>The current <see cref="Result{TError}"/>.</returns>
    public Result<TError> TapErrors(Action<List<TError>> onErrors)
    {
        ArgumentNullException.ThrowIfNull(onErrors);

        if (IsFailure)
        {
            onErrors(Errors);
        }

        return this;
    }

    /// <summary>
    /// Ensures the result is a success. Throws <see cref="InvalidOperationException"/> if it is a failure.
    /// </summary>
    /// <returns>The current <see cref="Result{TError}"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
    public Result<TError> ThrowIfFailure()
    {
        if (!IsSuccess)
        {
            var errorSummary = string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Description}"));
            var targets = Errors
                .Where(e => e.Target is not null)
                .Select(e => e.Target!)
                .ToList();

            if (targets.Count > 0)
            {
                throw new ArgumentException(
                    $"Errors({Errors.Count}): [{errorSummary}]",
                    string.Join(", ", targets));
            }

            throw new InvalidOperationException(
                $"Errors({Errors.Count}): [{errorSummary}]");
        }

        return this;
    }

    /// <summary>
    /// Transforms the error type using the specified mapper function.
    /// If the result is a success, a success of the new error type is returned.
    /// If the result is a failure, each error is transformed and returned as a failure.
    /// </summary>
    /// <typeparam name="TNewError">The type of the transformed error.</typeparam>
    /// <param name="mapper">The function to transform each error.</param>
    /// <returns>A new <see cref="Result{TNewError}"/> with the mapped errors or propagated success.</returns>
    public Result<TNewError> MapError<TNewError>(
        Func<TError, TNewError> mapper)
        where TNewError : IError
    {
        ArgumentNullException.ThrowIfNull(mapper);

        if (IsSuccess)
        {
            var success = Result<TNewError>.Success(Status);

            return Metadata is not null
                ? success.WithResultMeta(Metadata)
                : success;
        }

        var newErrors = Errors.Select(mapper).ToList();
        var failure = Result<TNewError>.Failure(newErrors);

        return Metadata is not null
            ? failure.WithResultMeta(Metadata)
            : failure;
    }

    /// <summary>
    /// Returns a failure result with the specified error if the predicate is true.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate against the success state.</param>
    /// <param name="error">The error to return when the predicate is true.</param>
    /// <returns>A failure result if the predicate is true, otherwise the original result.</returns>
    public Result<TError> FailIf(
        Func<bool> predicate,
        TError error)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (IsFailure)
        {
            return this;
        }

        return predicate()
            ? Failure(error)
            : this;
    }

    /// <summary>
    /// Returns a failure result using the error factory if the predicate is true.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate against the success state.</param>
    /// <param name="errorFactory">The factory to create the error when the predicate is true.</param>
    /// <returns>A failure result if the predicate is true, otherwise the original result.</returns>
    public Result<TError> FailIf(
        Func<bool> predicate,
        Func<TError> errorFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorFactory);

        if (IsFailure)
        {
            return this;
        }

        return predicate()
            ? Failure(errorFactory())
            : this;
    }

    /// <summary>
    /// Validates the result using the specified predicate.
    /// Returns a failure result with the specified error if the predicate returns false.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="predicate">The predicate that must return true for validation to pass.</param>
    /// <param name="error">The error to return when validation fails.</param>
    /// <returns>The original result if validation passes, otherwise a failure result.</returns>
    public Result<TError> Ensure(
        Func<bool> predicate,
        TError error)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (IsFailure)
        {
            return this;
        }

        return predicate()
            ? this
            : Failure(error);
    }

    /// <summary>
    /// Validates the result using a validator function that returns a result.
    /// If the validator returns a failure, that failure is propagated.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="validator">The validator function that returns a result.</param>
    /// <returns>The original result if validation passes, otherwise the validator's failure result.</returns>
    public Result<TError> Ensure(
        Func<Result<TError>> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);

        if (IsFailure)
        {
            return this;
        }

        var result = validator();

        return result.IsFailure
            ? result
            : this;
    }

    /// <summary>
    /// Executes the appropriate action based on the result state.
    /// Unlike <see cref="Match{TResult}"/>, this method does not return a value.
    /// </summary>
    /// <param name="onSuccess">The action to execute on success.</param>
    /// <param name="onFailure">The action to execute with the errors.</param>
    public void Switch(
        Action onSuccess,
        Action<IReadOnlyList<TError>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onFailure(Errors);
        }
    }

    /// <summary>
    /// Executes the specified action if the result is a failure, passing the first error.
    /// Returns the original result to enable fluent chaining.
    /// </summary>
    /// <param name="onError">The action to execute with the first error.</param>
    /// <returns>The current <see cref="Result{TError}"/>.</returns>
    public Result<TError> TapError(Action<TError> onError)
    {
        ArgumentNullException.ThrowIfNull(onError);

        if (IsFailure && Errors.Count > 0)
        {
            onError(Errors[0]);
        }

        return this;
    }

    #endregion
}
