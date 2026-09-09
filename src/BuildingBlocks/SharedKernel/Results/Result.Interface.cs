using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.SharedKernel.Results;

/// <summary>
/// Represents the result of an operation that may contain errors but does not produce a value.
/// </summary>
/// <typeparam name="TError">The type of error contained in the result.</typeparam>
public interface IResult<TError> : IMetadata
    where TError : IError
{
    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets the HTTP status code associated with the result.
    /// </summary>
    int Status { get; }

    /// <summary>
    /// Gets the list of errors. Empty if the operation succeeded.
    /// </summary>
    List<TError> Errors { get; }

}
