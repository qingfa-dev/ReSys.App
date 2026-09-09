using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.SharedKernel.Results;

/// <summary>
/// Represents the result of an operation that may contain errors but does not produce a value.
/// Provides functional methods for composing result pipelines.
/// </summary>
/// <typeparam name="TError">The type of error contained in the result.</typeparam>
public partial record Result<TError> : IResult<TError>
    where TError : IError
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TError}"/> record.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <param name="errors">The list of errors.</param>
    [JsonConstructor]
    protected Result(
        bool isSuccess,
        int status,
        List<TError> errors)
    {
        ResultValidator.ValidateErrors(errors, isFailure: !isSuccess);
        IsSuccess = isSuccess;
        Status = status;
        Errors = errors;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Errors))]
    [MemberNotNullWhen(false, nameof(Errors))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Errors))]
    [MemberNotNullWhen(true, nameof(Errors))]
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the HTTP status code associated with the result.
    /// </summary>
    public int Status { get; }

    /// <summary>
    /// Gets the list of errors.
    /// This collection is empty when the operation succeeds and contains at least one error when it fails.
    /// </summary>
    public List<TError> Errors { get; }

    /// <summary>
    /// Gets optional metadata associated with the result.
    /// </summary>
    public IMetadataDictionary Metadata { get; init; } = MetadataDictionary.Create();

    #endregion

    #region Fluent Methods

    /// <summary>
    /// Sets metadata on the result. Returns the same result instance with metadata attached.
    /// </summary>
    /// <param name="metadata">The metadata to attach.</param>
    /// <returns>The current <see cref="Result{TError}"/> with metadata set.</returns>
    public Result<TError> WithResultMeta(
        IReadOnlyDictionary<string, object?> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        var copiedMetadata = MetadataDictionary.Create();
        foreach (var (key, value) in metadata)
        {
            copiedMetadata[key] = value!;
        }

        return this with
        {
            Metadata = copiedMetadata
        };
    }

    public Result<TError> WithResultMeta(IMetadataDictionary metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        return this with { Metadata = metadata };
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString()
    {
        if (IsSuccess)
        {
            return $"Success({Status})";
        }

        var errorSummary = Errors?.Count > 0
            ? string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Description}"))
            : "None";

        return $"Failure({Status}, Errors({Errors?.Count}): [{errorSummary}])";
    }

    #endregion
}