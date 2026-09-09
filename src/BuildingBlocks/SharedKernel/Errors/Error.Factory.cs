namespace BuildingBlocks.SharedKernel.Errors;

public partial record Error : IError
{
    #region Factory Methods

    /// <summary>
    /// Creates an error with the specified properties.
    /// </summary>
    /// <param name="type">The error type URI.</param>
    /// <param name="code">The machine-readable error code.</param>
    /// <param name="description">The human-readable error description.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    public static Error Create(
        string type,
        string code,
        string description,
        int status,
        string? target = null)
    {
        return new Error(type, code, description, status, target);
    }

    #endregion
}