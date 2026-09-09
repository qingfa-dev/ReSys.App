namespace BuildingBlocks.SharedKernel.ValueResults;

/// <summary>
/// Provides constants for ValueResult type constraints and validation messages.
/// </summary>
public static class ValueResultConstant
{
    /// <summary>
    /// Validation constraints for ValueResult types.
    /// </summary>
    public static class Constraints
    {
        /// <summary>
        /// Maximum number of metadata entries allowed.
        /// </summary>
        public const int MaxMetadataEntries = 50;
    }

    /// <summary>
    /// Error type URIs for ValueResult validation failures.
    /// </summary>
    public static class ErrorTypes
    {
        /// <summary>Validation error.</summary>
        public const string ValidationError =
            "https://problems-registry.smartbear.com/validation-error";

        /// <summary>Conflict error.</summary>
        public const string Conflict =
            "https://problems-registry.smartbear.com/conflict";
    }

    /// <summary>
    /// Error codes for ValueResult validation failures.
    /// </summary>
    public static class Codes
    {
        /// <summary>Value is required for success result.</summary>
        public const string ValueRequired =
            "ValueResult.ValueRequired";

        /// <summary>Value must be null for failure result.</summary>
        public const string ValueNotAllowed =
            "ValueResult.ValueNotAllowed";

        /// <summary>Status code must be in valid HTTP range.</summary>
        public const string InvalidStatusCode =
            "ValueResult.InvalidStatusCode";

        /// <summary>Success result must have 2xx status code.</summary>
        public const string InvalidSuccessStatus =
            "ValueResult.InvalidSuccessStatus";

        /// <summary>Failure result must have 4xx/5xx status code.</summary>
        public const string InvalidFailureStatus =
            "ValueResult.InvalidFailureStatus";
    }

    /// <summary>
    /// Error messages for ValueResult validation failures.
    /// </summary>
    public static class Messages
    {
        /// <summary>Value is required for success result.</summary>
        public const string ValueRequired =
            "A success result must have a non-null value.";

        /// <summary>Value must be null for failure result.</summary>
        public const string ValueNotAllowed =
            "A failure result cannot have a value.";

        /// <summary>Status code must be in valid HTTP range.</summary>
        public const string InvalidStatusCode =
            "Status code must be between 100 and 599, but was {0}.";

        /// <summary>Success result must have 2xx status code.</summary>
        public const string InvalidSuccessStatus =
            "Success result must have a 2xx status code, but was {0}.";

        /// <summary>Failure result must have 4xx/5xx status code.</summary>
        public const string InvalidFailureStatus =
            "Failure result must have a 4xx or 5xx status code, but was {0}.";
    }
}
