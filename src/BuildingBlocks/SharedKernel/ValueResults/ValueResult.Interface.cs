namespace BuildingBlocks.SharedKernel.ValueResults;

public interface IValueOf<out TType>
{
    /// <summary>
    /// Gets the success value, or <see langword="default"/> if the result is a failure.
    /// </summary>
    TType Value { get; }
}

/// <summary>
/// Represents the result of an operation that produces a value and may contain errors.
/// Extends <see cref="IResult{TError}"/> with an optional success value.
/// </summary>
/// <typeparam name="TValue">The type of the success value.</typeparam>
/// <typeparam name="TError">The type of error contained in the result.</typeparam>
public interface IResult<out TValue, TError> : IResult<TError>, IValueOf<TValue>
    where TError : IError;