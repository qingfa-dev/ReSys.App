namespace BuildingBlocks.SharedKernel.Errors;

/// <summary>
/// Represents an error with a status code, type, code, description, and optional target.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets the HTTP status code associated with the error.
    /// </summary>
    int Status { get; }

    /// <summary>
    /// Gets the error type URI (e.g., "https://problems-registry.smartbear.com/bad-request").
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Gets the machine-readable error code (e.g., "Request.MissingBodyProperty").
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Gets the human-readable error description.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the target that caused the error, if applicable (e.g., a field name or endpoint).
    /// </summary>
    string? Target { get; }
}
