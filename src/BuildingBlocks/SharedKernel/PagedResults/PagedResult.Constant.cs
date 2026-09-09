namespace BuildingBlocks.SharedKernel.PagedResults;

/// <summary>
/// Provides constants for PagedResult validation and defaults.
/// </summary>
public static class PagedResultConstant
{
    /// <summary>
    /// Default values for pagination.
    /// </summary>
    public static class Defaults
    {
        /// <summary>Default page number.</summary>
        public const int PageNumber = 1;

        /// <summary>Default page size.</summary>
        public const int PageSize = 10;

        /// <summary>Default total count.</summary>
        public const long TotalCount = 0;
    }

    /// <summary>
    /// Validation constraints for pagination.
    /// </summary>
    public static class Constraints
    {
        /// <summary>Minimum page number.</summary>
        public const int MinPageNumber = 1;

        /// <summary>Minimum page size.</summary>
        public const int MinPageSize = 1;

        /// <summary>Maximum page size.</summary>
        public const int MaxPageSize = 1000;
    }

    /// <summary>
    /// Error type URIs for PagedResult validation failures.
    /// </summary>
    public static class ErrorTypes
    {
        /// <summary>Validation error.</summary>
        public const string ValidationError =
            "https://problems-registry.smartbear.com/validation-error";
    }

    /// <summary>
    /// Error codes for PagedResult validation failures.
    /// </summary>
    public static class Codes
    {
        /// <summary>Invalid page number.</summary>
        public const string InvalidPageNumber =
            "PagedResult.InvalidPageNumber";

        /// <summary>Invalid page size.</summary>
        public const string InvalidPageSize =
            "PagedResult.InvalidPageSize";

        /// <summary>Invalid total count.</summary>
        public const string InvalidTotalCount =
            "PagedResult.InvalidTotalCount";
    }

    /// <summary>
    /// Error messages for PagedResult validation failures.
    /// </summary>
    public static class Messages
    {
        /// <summary>Invalid page number message.</summary>
        public const string InvalidPageNumber =
            "Page number must be greater than 0, but was {0}.";

        /// <summary>Invalid page size message.</summary>
        public const string InvalidPageSize =
            "Page size must be between {0} and {1}, but was {2}.";

        /// <summary>Invalid total count message.</summary>
        public const string InvalidTotalCount =
            "Total count must be non-negative, but was {0}.";
    }
}