using System.Text.Json.Serialization;

using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.SharedKernel.PagedResults;

/// <summary>
/// Represents the result of an operation that returns a paged collection of values
/// and may contain one or more errors. Inherits from <see cref="Result{TValue, TError}"/>
/// with <c>TValue = IReadOnlyList&lt;TItem&gt;</c>. Metadata is never null.
/// </summary>
/// <typeparam name="TValue">The type of item contained in the paged result.</typeparam>
/// <typeparam name="TError">The type of error contained in the result.</typeparam>
public partial record PagedResult<TValue, TError> :
    Result<IReadOnlyList<TValue>, TError>,
    IPagedResult<TValue, TError>
    where TError : IError
{
    #region Constructor

    [JsonConstructor]
    private PagedResult(
        IReadOnlyList<TValue> value,
        bool isSuccess,
        int status,
        List<TError> errors,
        PagedMetadata pagedMetadata)
        : base(
            value,
            isSuccess,
            status,
            errors)
    {
        ArgumentNullException.ThrowIfNull(pagedMetadata);
        PagedMetadata = pagedMetadata;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the paged metadata. Never null.
    /// </summary>
    public PagedMetadata PagedMetadata { get; init; }

    /// <summary>
    /// Gets the shared result metadata. Never null for paged results.
    /// </summary>
    public new IMetadataDictionary Metadata => PagedMetadata.Metadata;

    /// <inheritdoc />
    [JsonIgnore]
    IPagedMetadata IPagedResult<TValue, TError>.PagedMetadata => PagedMetadata;

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString()
    {
        if (IsSuccess)
        {
            return $"Success({Status}, Page: {PagedMetadata.PageNumber}/{PagedMetadata.TotalPages}, Items: {PagedMetadata.ItemCount})";
        }

        var errorSummary = Errors?.Count > 0
            ? string.Join("; ", Errors.Select(e => $"{e.Code}: {e.Description}"))
            : "None";

        return $"Failure({Status}, Errors({Errors?.Count}): [{errorSummary}])";
    }

    #endregion
}