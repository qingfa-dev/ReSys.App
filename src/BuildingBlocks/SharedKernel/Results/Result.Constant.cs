namespace BuildingBlocks.SharedKernel.Results;

/// <summary>
/// Provides constants for Result type constraints, defaults, and validation messages.
/// </summary>
public static class ResultConstant
{
    /// <summary>
    /// Validation constraints for Result types.
    /// </summary>
    public static class Constraints
    {
        /// <summary>
        /// Maximum number of errors allowed in a single result.
        /// </summary>
        public const int MaxErrors = 100;
    }

    /// <summary>
    /// Error type URIs for Result validation failures.
    /// </summary>
    public static class ErrorTypes
    {
        /// <summary>Validation error.</summary>
        public const string ValidationError =
            "https://problems-registry.smartbear.com/validation-error";
    }

    /// <summary>
    /// Error codes for Result validation failures.
    /// </summary>
    public static class Codes
    {
        /// <summary>Empty error list.</summary>
        public const string EmptyErrors =
            "Result.EmptyErrors";

        /// <summary>Exceeds maximum error count.</summary>
        public const string ExceedsMaxErrors =
            "Result.ExceedsMaxErrors";

        /// <summary>Mixed status codes.</summary>
        public const string MixedStatusCodes =
            "Result.MixedStatusCodes";

        /// <summary>Failed result must contain at least one error.</summary>
        public const string FailureWithoutErrors =
            "Result.FailureWithoutErrors";
    }

    /// <summary>
    /// Error messages for Result validation failures.
    /// </summary>
    public static class Messages
    {
        /// <summary>At least one error is required.</summary>
        public const string EmptyErrors =
            "At least one error is required.";

        /// <summary>A result cannot contain more than N errors.</summary>
        public const string ExceedsMaxErrors =
            "A result cannot contain more than {0} errors.";

        /// <summary>All errors in a result must have the same HTTP status code.</summary>
        public const string MixedStatusCodes =
            "All errors in a result must have the same HTTP status code.";
    }
}