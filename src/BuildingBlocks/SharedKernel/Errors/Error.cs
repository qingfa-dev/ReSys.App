using System.Text.Json.Serialization;

namespace BuildingBlocks.SharedKernel.Errors;

/// <summary>
/// Represents an error with a status code, type, code, description, and optional target.
/// Provides static factory methods for creating common HTTP error types.
/// </summary>
public partial record Error : IError
{
    #region Properties

    /// <summary>
    /// Gets the HTTP status code associated with the error.
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; }

    /// <summary>
    /// Gets the error type URI (e.g., "https://problems-registry.smartbear.com/bad-request").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; }

    /// <summary>
    /// Gets the machine-readable error code (e.g., "Request.MissingBodyProperty").
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; }

    /// <summary>
    /// Gets the human-readable error description.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }

    /// <summary>
    /// Gets the target that caused the error, if applicable.
    /// </summary>
    [JsonPropertyName("target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Target { get; }

    #endregion

    #region Constructor

    [JsonConstructor]
    private Error(
        string type,
        string code,
        string description,
        int status,
        string? target)
    {
        ErrorValidator.ValidateStatus(status);
        ErrorValidator.ValidateType(type);
        ErrorValidator.ValidateCode(code);
        ErrorValidator.ValidateDescription(description);

        Status = status;
        Type = type;
        Code = code;
        Description = description;
        Target = target;
    }

    #endregion
}