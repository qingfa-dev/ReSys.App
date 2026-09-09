using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.SharedKernel.ValueResults;

/// <summary>
/// Represents the result of an operation that produces a value and may contain errors.
/// Provides functional methods for composing result pipelines.
/// </summary>
/// <typeparam name="TValue">The type of the success value.</typeparam>
/// <typeparam name="TError">The type of error contained in the result.</typeparam>
public partial record Result<TValue, TError> : IResult<TValue, TError>
    where TError : IError
{
    #region Constructor

    [JsonConstructor]
    protected Result(
        TValue? value,
        bool isSuccess,
        int status,
        List<TError> errors)
        : base(
            isSuccess,
            status,
            errors)
    {
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the success value, or <see langword="default"/> if the result is a failure.
    /// </summary>
    [AllowNull]
    public TValue Value
    {
        get => field!;
        set;
    }

    /// <summary>
    /// Gets a value indicating whether the result has a non-null success value.
    /// </summary>
    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue => IsSuccess && Value is not null;

    #endregion

    #region Fluent Methods

    /// <summary>
    /// Sets metadata on the result. Returns the same result instance with metadata attached.
    /// </summary>
    /// <param name="metadata">The metadata to attach.</param>
    /// <returns>The current <see cref="Result{TValue, TError}"/> with metadata set.</returns>
    public new Result<TValue, TError> WithResultMeta(
        IReadOnlyDictionary<string, object?> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        var copiedMetadata = MetadataDictionary.Create();
        foreach (var (key, value) in metadata)
        {
            copiedMetadata[key] = value!;
        }

        return this with { Metadata = copiedMetadata };
    }

    public new Result<TValue, TError> WithResultMeta(IMetadataDictionary metadata)
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
            return $"Success({Status}, Value: {Value})";
        }

        var errorSummary = Errors?.Count > 0
            ? string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Description}"))
            : "None";

        return $"Failure({Status}, Errors({Errors?.Count}): [{errorSummary}])";
    }

    #endregion
}