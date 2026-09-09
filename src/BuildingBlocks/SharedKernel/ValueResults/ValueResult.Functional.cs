namespace BuildingBlocks.SharedKernel.ValueResults;

public partial record Result<TValue, TError> : Result<TError>, IResult<TValue, TError>
    where TError : IError
{
    #region Functional Methods

    /// <summary>
    /// Transforms the success value using the specified mapper function.
    /// If the result is a failure, the errors are propagated without transformation.
    /// </summary>
    /// <typeparam name="TNewValue">The type of the transformed value.</typeparam>
    /// <param name="mapper">The function to transform the success value.</param>
    /// <returns>A new <see cref="Result{TNewValue, TError}"/> with the mapped value or propagated errors.</returns>
    public Result<TNewValue, TError> Map<TNewValue>(
        Func<TValue, TNewValue> mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper);

        if (!HasValue)
        {
            var failure = Result<TNewValue, TError>.Failure(
                Errors);

            return Metadata is not null
                ? failure.WithResultMeta(Metadata)
                : failure;
        }

        var success = Result<TNewValue, TError>.Success(
            mapper(Value));

        return Metadata is not null
            ? success.WithResultMeta(Metadata)
            : success;
    }

    /// <summary>
    /// Chains the result with a function that produces a new result from the success value.
    /// If the result is a failure, the errors are propagated without invoking the binder.
    /// </summary>
    /// <typeparam name="TNewValue">The type of the value in the new result.</typeparam>
    /// <param name="binder">The function to produce a new result from the success value.</param>
    /// <returns>The result produced by the binder, or a failure with propagated errors.</returns>
    public Result<TNewValue, TError> Bind<TNewValue>(
        Func<TValue, Result<TNewValue, TError>> binder)
    {
        ArgumentNullException.ThrowIfNull(binder);

        if (!HasValue)
        {
            var failure = Result<TNewValue, TError>.Failure(
                Errors);

            return Metadata is not null
                ? failure.WithResultMeta(Metadata)
                : failure;
        }

        return binder(Value);
    }

    /// <summary>
    /// Matches the result to one of two functions: one for success and one for failure.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match functions.</typeparam>
    /// <param name="onSuccess">The function to invoke when the result is a success.</param>
    /// <param name="onFailure">The function to invoke when the result is a failure.</param>
    /// <returns>The value produced by the invoked function.</returns>
    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<IReadOnlyList<TError>, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return HasValue
            ? onSuccess(Value)
            : onFailure(Errors);
    }

    /// <summary>
    /// Executes the specified action if the result is a success, passing the success value.
    /// Returns the original result to enable fluent chaining.
    /// </summary>
    /// <param name="onSuccess">The action to execute with the success value.</param>
    /// <returns>The original <see cref="Result{TValue, TError}"/>.</returns>
    public Result<TValue, TError> Tap(Action<TValue> onSuccess)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        if (HasValue)
        {
            onSuccess(Value);
        }

        return this;
    }

    /// <summary>
    /// Executes the specified action if the result is a failure, passing the first error.
    /// Returns the original result to enable fluent chaining.
    /// </summary>
    /// <param name="onErrors">The action to execute with the first error.</param>
    /// <returns>The original <see cref="Result{TValue, TError}"/>.</returns>
    public new Result<TValue, TError> TapErrors(Action<List<TError>> onErrors)
    {
        ArgumentNullException.ThrowIfNull(onErrors);

        if (IsFailure && Errors is not null)
        {
            onErrors(Errors);
        }

        return this;
    }

    /// <summary>
    /// Returns the success value, or the specified default value if the result is a failure.
    /// </summary>
    /// <param name="defaultValue">The default value to return on failure.</param>
    /// <returns>The success value or the default value.</returns>
    public TValue GetValueOrDefault(TValue defaultValue = default!)
    {
        return HasValue ? Value : defaultValue;
    }

    /// <summary>
    /// Returns the success value, or throws an <see cref="InvalidOperationException"/> if the result is a failure.
    /// </summary>
    /// <returns>The success value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
    public TValue GetValueOrThrow()
    {
        if (!HasValue)
        {
            var errorSummary = string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException(
                $"Errors({Errors.Count}): [{errorSummary}]");
        }

        return Value;
    }

    /// <summary>
    /// Ensures the result is a success. Throws <see cref="InvalidOperationException"/> if it is a failure.
    /// </summary>
    /// <returns>The current <see cref="Result{TValue, TError}"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
    public new Result<TValue, TError> ThrowIfFailure()
    {
        if (!IsSuccess)
        {
            var errorSummary = string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException(
                $"Errors({Errors.Count}): [{errorSummary}]");
        }

        return this;
    }

    /// <summary>
    /// Transforms the error using the specified mapper function.
    /// If the result is a success, the value is propagated without transformation.
    /// </summary>
    /// <typeparam name="TNewError">The type of the transformed error.</typeparam>
    /// <param name="mapper">The function to transform the error.</param>
    /// <returns>A new <see cref="Result{TValue, TNewError}"/> with the mapped error or propagated value.</returns>
    public new Result<TValue, TNewError> MapError<TNewError>(
        Func<TError, TNewError> mapper)
        where TNewError : IError
    {
        ArgumentNullException.ThrowIfNull(mapper);

        if (HasValue)
        {
            var success = Result<TValue, TNewError>.Success(
                Value);

            return Metadata is not null
                ? success.WithResultMeta(Metadata)
                : success;
        }

        var newErrors = Errors.Select(mapper).ToList();
        var failure = Result<TValue, TNewError>.Failure(
            newErrors);

        return Metadata is not null
            ? failure.WithResultMeta(Metadata)
            : failure;
    }

    /// <summary>
    /// Returns a failure result with the specified error if the predicate is true.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate against the success value.</param>
    /// <param name="error">The error to return when the predicate is true.</param>
    /// <returns>A failure result if the predicate is true, otherwise the original result.</returns>
    public Result<TValue, TError> FailIf(
        Func<TValue, bool> predicate,
        TError error)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (IsFailure)
        {
            return this;
        }

        return predicate(Value)
            ? Failure(error)
            : this;
    }

    /// <summary>
    /// Returns a failure result using the error factory if the predicate is true.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate against the success value.</param>
    /// <param name="errorFactory">The factory to create the error when the predicate is true.</param>
    /// <returns>A failure result if the predicate is true, otherwise the original result.</returns>
    public Result<TValue, TError> FailIf(
        Func<TValue, bool> predicate,
        Func<TValue, TError> errorFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorFactory);

        if (IsFailure)
        {
            return this;
        }

        return predicate(Value)
            ? Failure(errorFactory(Value))
            : this;
    }

    /// <summary>
    /// Validates the success value using the specified predicate.
    /// Returns a failure result with the specified error if the predicate returns false.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="predicate">The predicate that must return true for validation to pass.</param>
    /// <param name="error">The error to return when validation fails.</param>
    /// <returns>The original result if validation passes, otherwise a failure result.</returns>
    public Result<TValue, TError> Ensure(
        Func<TValue, bool> predicate,
        TError error)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (IsFailure)
        {
            return this;
        }

        return predicate(Value)
            ? this
            : Failure(error);
    }

    /// <summary>
    /// Validates the success value using a validator function that returns a result.
    /// If the validator returns a failure, that failure is propagated.
    /// If the result is already a failure, it is returned unchanged.
    /// </summary>
    /// <param name="validator">The validator function that returns a result.</param>
    /// <returns>The original result if validation passes, otherwise the validator's failure result.</returns>
    public Result<TValue, TError> Ensure(
        Func<TValue, Result<TValue, TError>> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);

        if (IsFailure)
        {
            return this;
        }

        var result = validator(Value);

        return result.IsFailure
            ? result
            : this;
    }

    /// <summary>
    /// Executes the appropriate action based on the result state.
    /// Unlike <see cref="Match{TResult}"/>, this method does not return a value.
    /// </summary>
    /// <param name="onSuccess">The action to execute with the success value.</param>
    /// <param name="onFailure">The action to execute with the errors.</param>
    public void Switch(
        Action<TValue> onSuccess,
        Action<IReadOnlyList<TError>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (HasValue)
        {
            onSuccess(Value);
        }
        else
        {
            onFailure(Errors);
        }
    }

    #endregion
}
