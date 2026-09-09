namespace BuildingBlocks.SharedKernel.Errors;

/// <summary>
/// Provides constants for error constraints, defaults, and predefined error types.
/// </summary>
public static class ErrorConstant
{
    /// <summary>
    /// Validation constraints for error properties.
    /// </summary>
    public static class Constraints
    {
        /// <summary>
        /// Maximum length for an error code.
        /// </summary>
        public const int MaxCodeLength = 256;

        /// <summary>
        /// Maximum length for an error description.
        /// </summary>
        public const int MaxDescriptionLength = 2048;
    }

    /// <summary>
    /// Default values for error properties.
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// Default error type URI.
        /// </summary>
        public const string Type =
            "https://problems-registry.smartbear.com/server-error";

        /// <summary>
        /// Default error code.
        /// </summary>
        public const string Code =
            "General.Unexpected";

        /// <summary>
        /// Default error message.
        /// </summary>
        public const string Message =
            "An unexpected error occurred.";
    }

    /// <summary>
    /// Predefined error type URIs for common HTTP status codes.
    /// </summary>
    public static class ErrorTypes
    {
        /// <summary>400 Bad Request.</summary>
        public const string BadRequest =
            "https://problems-registry.smartbear.com/bad-request";

        /// <summary>400 Bad Request - Missing body property.</summary>
        public const string MissingBodyProperty =
            "https://problems-registry.smartbear.com/missing-body-property";

        /// <summary>400 Bad Request - Missing request header.</summary>
        public const string MissingRequestHeader =
            "https://problems-registry.smartbear.com/missing-request-header";

        /// <summary>400 Bad Request - Missing request parameter.</summary>
        public const string MissingRequestParameter =
            "https://problems-registry.smartbear.com/missing-request-parameter";

        /// <summary>400 Bad Request - Invalid body property format.</summary>
        public const string InvalidBodyPropertyFormat =
            "https://problems-registry.smartbear.com/invalid-body-property-format";

        /// <summary>400 Bad Request - Invalid request parameter format.</summary>
        public const string InvalidRequestParameterFormat =
            "https://problems-registry.smartbear.com/invalid-request-parameter-format";

        /// <summary>400 Bad Request - Invalid request header format.</summary>
        public const string InvalidRequestHeaderFormat =
            "https://problems-registry.smartbear.com/invalid-request-header-format";

        /// <summary>400 Bad Request - Invalid body property value.</summary>
        public const string InvalidBodyPropertyValue =
            "https://problems-registry.smartbear.com/invalid-body-property-value";

        /// <summary>400 Bad Request - Invalid request parameter value.</summary>
        public const string InvalidRequestParameterValue =
            "https://problems-registry.smartbear.com/invalid-request-parameter-value";

        /// <summary>409 Conflict - Resource already exists.</summary>
        public const string AlreadyExists =
            "https://problems-registry.smartbear.com/already-exists";

        /// <summary>422 Unprocessable Entity - Validation error.</summary>
        public const string ValidationError =
            "https://problems-registry.smartbear.com/validation-error";

        /// <summary>422 Unprocessable Entity - Business rule violation.</summary>
        public const string BusinessRuleViolation =
            "https://problems-registry.smartbear.com/business-rule-violation";

        /// <summary>503 Service Unavailable - License expired.</summary>
        public const string LicenseExpired =
            "https://problems-registry.smartbear.com/license-expired";

        /// <summary>503 Service Unavailable - License cancelled.</summary>
        public const string LicenseCancelled =
            "https://problems-registry.smartbear.com/license-cancelled";

        /// <summary>404 Not Found.</summary>
        public const string NotFound =
            "https://problems-registry.smartbear.com/not-found";

        /// <summary>401 Unauthorized.</summary>
        public const string Unauthorized =
            "https://problems-registry.smartbear.com/unauthorized";

        /// <summary>403 Forbidden.</summary>
        public const string Forbidden =
            "https://problems-registry.smartbear.com/forbidden";

        /// <summary>400 Bad Request - Invalid parameters.</summary>
        public const string InvalidParameters =
            "https://problems-registry.smartbear.com/invalid-parameters";

        /// <summary>503 Service Unavailable.</summary>
        public const string ServiceUnavailable =
            "https://problems-registry.smartbear.com/service-unavailable";

        /// <summary>500 Internal Server Error.</summary>
        public const string ServerError =
            "https://problems-registry.smartbear.com/server-error";

        /// <summary>500 Internal Server Error - General failure.</summary>
        public const string Failure =
            "https://problems-registry.smartbear.com/failure";
    }

    /// <summary>
    /// Predefined error codes for common error conditions.
    /// </summary>
    public static class Codes
    {
        /// <summary>Missing body property code.</summary>
        public const string MissingBodyProperty =
            "Request.MissingBodyProperty";

        /// <summary>Missing request header code.</summary>
        public const string MissingRequestHeader =
            "Request.MissingHeader";

        /// <summary>Missing request parameter code.</summary>
        public const string MissingRequestParameter =
            "Request.MissingParameter";

        /// <summary>Invalid body property format code.</summary>
        public const string InvalidBodyPropertyFormat =
            "Request.InvalidBodyPropertyFormat";

        /// <summary>Invalid body property value code.</summary>
        public const string InvalidBodyPropertyValue =
            "Request.InvalidBodyPropertyValue";

        /// <summary>Invalid request parameter format code.</summary>
        public const string InvalidRequestParameterFormat =
            "Request.InvalidParameterFormat";

        /// <summary>Invalid request parameter value code.</summary>
        public const string InvalidRequestParameterValue =
            "Request.InvalidParameterValue";

        /// <summary>Invalid request header format code.</summary>
        public const string InvalidRequestHeaderFormat =
            "Request.InvalidHeaderFormat";

        /// <summary>Invalid parameters code.</summary>
        public const string InvalidParameters =
            "Request.InvalidParameters";

        /// <summary>General failure code.</summary>
        public const string Failure =
            "General.Failure";

        /// <summary>Invalid status code.</summary>
        public const string InvalidStatus =
            "Error.InvalidStatus";

        /// <summary>Code exceeds maximum length.</summary>
        public const string CodeMaxLength =
            "Error.CodeMaxLength";

        /// <summary>Description exceeds maximum length.</summary>
        public const string DescriptionMaxLength =
            "Error.DescriptionMaxLength";
    }

    /// <summary>
    /// Error description format strings. Use <see cref="string.Format(string,object)"/> to interpolate arguments.
    /// </summary>
    public static class Messages
    {
        /// <summary>Body property is required.</summary>
        public const string MissingBodyProperty =
            "The body property '{0}' is required.";

        /// <summary>Request header is required.</summary>
        public const string MissingRequestHeader =
            "The request header '{0}' is required.";

        /// <summary>Request parameter is required.</summary>
        public const string MissingRequestParameter =
            "The request parameter '{0}' is required.";

        /// <summary>Body property has invalid format.</summary>
        public const string InvalidBodyPropertyFormat =
            "The body property '{0}' has an invalid format.";

        /// <summary>Body property has invalid value.</summary>
        public const string InvalidBodyPropertyValue =
            "The body property '{0}' has an invalid value.";

        /// <summary>Request parameter has invalid format.</summary>
        public const string InvalidRequestParameterFormat =
            "The request parameter '{0}' has an invalid format.";

        /// <summary>Request parameter has invalid value.</summary>
        public const string InvalidRequestParameterValue =
            "The request parameter '{0}' has an invalid value.";

        /// <summary>Request header has invalid format.</summary>
        public const string InvalidRequestHeaderFormat =
            "The request header '{0}' has an invalid format.";

        /// <summary>Status must be within valid HTTP range.</summary>
        public const string InvalidStatus =
            "Status must be a valid HTTP status code between 100 and 599, but was {0}.";

        /// <summary>Code exceeds maximum length.</summary>
        public const string CodeMaxLength =
            "Code cannot exceed {0} characters.";

        /// <summary>Description exceeds maximum length.</summary>
        public const string DescriptionMaxLength =
            "Description cannot exceed {0} characters.";

        /// <summary>General failure description.</summary>
        public const string Failure =
            "The operation failed.";
    }
}