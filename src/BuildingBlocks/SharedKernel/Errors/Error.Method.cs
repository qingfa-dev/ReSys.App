using System.Net;

namespace BuildingBlocks.SharedKernel.Errors;

public partial record Error : IError
{
    #region Server Errors

    /// <summary>
    /// Creates a 500 Internal Server Error.
    /// </summary>
    /// <param name="code">The error code. Defaults to "General.Unexpected".</param>
    /// <param name="description">The error description. Defaults to "An unexpected error occurred.".</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 500 <see cref="Error"/>.</returns>
    public static Error Unexpected(
        string code = ErrorConstant.Defaults.Code,
        string description = ErrorConstant.Defaults.Message,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.ServerError,
            code,
            description,
            (int)HttpStatusCode.InternalServerError,
            target);
    }

    /// <summary>
    /// Creates a 500 Internal Server Error.
    /// </summary>
    /// <param name="code">The error code. Defaults to "General.Unexpected".</param>
    /// <param name="description">The error description. Defaults to "An unexpected error occurred.".</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 500 <see cref="Error"/>.</returns>
    public static Error ServerError(
        string code = ErrorConstant.Defaults.Code,
        string description = ErrorConstant.Defaults.Message,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.ServerError,
            code,
            description,
            (int)HttpStatusCode.InternalServerError,
            target);
    }

    /// <summary>
    /// Creates a 500 Internal Server Error for a general failure.
    /// </summary>
    /// <param name="code">The error code. Defaults to "General.Failure".</param>
    /// <param name="description">The error description. Defaults to "The operation failed.".</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 500 <see cref="Error"/>.</returns>
    public static Error Failure(
        string code = ErrorConstant.Codes.Failure,
        string description = ErrorConstant.Messages.Failure,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.Failure,
            code,
            description,
            (int)HttpStatusCode.InternalServerError,
            target);
    }

    #endregion

    #region Client Errors (4xx)

    /// <summary>
    /// Creates a 400 Bad Request error.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error BadRequest(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.BadRequest,
            code,
            description,
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 401 Unauthorized error.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 401 <see cref="Error"/>.</returns>
    public static Error Unauthorized(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.Unauthorized,
            code,
            description,
            (int)HttpStatusCode.Unauthorized,
            target);
    }

    /// <summary>
    /// Creates a 403 Forbidden error.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 403 <see cref="Error"/>.</returns>
    public static Error Forbidden(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.Forbidden,
            code,
            description,
            (int)HttpStatusCode.Forbidden,
            target);
    }

    /// <summary>
    /// Creates a 404 Not Found error.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 404 <see cref="Error"/>.</returns>
    public static Error NotFound(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.NotFound,
            code,
            description,
            (int)HttpStatusCode.NotFound,
            target);
    }

    /// <summary>
    /// Creates a 409 Conflict error.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 409 <see cref="Error"/>.</returns>
    public static Error AlreadyExists(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.AlreadyExists,
            code,
            description,
            (int)HttpStatusCode.Conflict,
            target);
    }

    /// <summary>
    /// Creates a 422 Unprocessable Entity error for validation failures.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 422 <see cref="Error"/>.</returns>
    public static Error Validation(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.ValidationError,
            code,
            description,
            (int)HttpStatusCode.UnprocessableEntity,
            target);
    }

    /// <summary>
    /// Creates a 422 Unprocessable Entity error for business rule violations.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 422 <see cref="Error"/>.</returns>
    public static Error BusinessRuleViolation(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.BusinessRuleViolation,
            code,
            description,
            (int)HttpStatusCode.UnprocessableEntity,
            target);
    }

    #endregion

    #region Request Validation Errors (400)

    /// <summary>
    /// Creates a 400 Bad Request error for a missing body property.
    /// </summary>
    /// <param name="property">The name of the missing property.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error MissingBodyProperty(
        string property,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property);

        return new Error(
            ErrorConstant.ErrorTypes.MissingBodyProperty,
            ErrorConstant.Codes.MissingBodyProperty,
            string.Format(ErrorConstant.Messages.MissingBodyProperty, property),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for a missing request header.
    /// </summary>
    /// <param name="header">The name of the missing header.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error MissingRequestHeader(
        string header,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(header);

        return new Error(
            ErrorConstant.ErrorTypes.MissingRequestHeader,
            ErrorConstant.Codes.MissingRequestHeader,
            string.Format(ErrorConstant.Messages.MissingRequestHeader, header),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for a missing request parameter.
    /// </summary>
    /// <param name="parameter">The name of the missing parameter.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error MissingRequestParameter(
        string parameter,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter);

        return new Error(
            ErrorConstant.ErrorTypes.MissingRequestParameter,
            ErrorConstant.Codes.MissingRequestParameter,
            string.Format(ErrorConstant.Messages.MissingRequestParameter, parameter),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for an invalid body property format.
    /// </summary>
    /// <param name="property">The name of the property with invalid format.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error InvalidBodyPropertyFormat(
        string property,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property);

        return new Error(
            ErrorConstant.ErrorTypes.InvalidBodyPropertyFormat,
            ErrorConstant.Codes.InvalidBodyPropertyFormat,
            string.Format(ErrorConstant.Messages.InvalidBodyPropertyFormat, property),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for an invalid body property value.
    /// </summary>
    /// <param name="property">The name of the property with invalid value.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error InvalidBodyPropertyValue(
        string property,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property);

        return new Error(
            ErrorConstant.ErrorTypes.InvalidBodyPropertyValue,
            ErrorConstant.Codes.InvalidBodyPropertyValue,
            string.Format(ErrorConstant.Messages.InvalidBodyPropertyValue, property),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for an invalid request parameter format.
    /// </summary>
    /// <param name="parameter">The name of the parameter with invalid format.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error InvalidRequestParameterFormat(
        string parameter,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter);

        return new Error(
            ErrorConstant.ErrorTypes.InvalidRequestParameterFormat,
            ErrorConstant.Codes.InvalidRequestParameterFormat,
            string.Format(ErrorConstant.Messages.InvalidRequestParameterFormat, parameter),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for an invalid request parameter value.
    /// </summary>
    /// <param name="parameter">The name of the parameter with invalid value.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error InvalidRequestParameterValue(
        string parameter,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter);

        return new Error(
            ErrorConstant.ErrorTypes.InvalidRequestParameterValue,
            ErrorConstant.Codes.InvalidRequestParameterValue,
            string.Format(ErrorConstant.Messages.InvalidRequestParameterValue, parameter),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for an invalid request header format.
    /// </summary>
    /// <param name="header">The name of the header with invalid format.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error InvalidRequestHeaderFormat(
        string header,
        string? target = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(header);

        return new Error(
            ErrorConstant.ErrorTypes.InvalidRequestHeaderFormat,
            ErrorConstant.Codes.InvalidRequestHeaderFormat,
            string.Format(ErrorConstant.Messages.InvalidRequestHeaderFormat, header),
            (int)HttpStatusCode.BadRequest,
            target);
    }

    /// <summary>
    /// Creates a 400 Bad Request error for invalid parameters.
    /// </summary>
    /// <param name="description">The error description.</param>
    /// <param name="code">The error code. Defaults to "Request.InvalidParameters".</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 400 <see cref="Error"/>.</returns>
    public static Error InvalidParameters(
        string description,
        string code = ErrorConstant.Codes.InvalidParameters,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.InvalidParameters,
            code,
            description,
            (int)HttpStatusCode.BadRequest,
            target);
    }

    #endregion

    #region Service Errors (5xx)

    /// <summary>
    /// Creates a 503 Service Unavailable error.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 503 <see cref="Error"/>.</returns>
    public static Error ServiceUnavailable(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.ServiceUnavailable,
            code,
            description,
            (int)HttpStatusCode.ServiceUnavailable,
            target);
    }

    #endregion

    #region License Errors

    /// <summary>
    /// Creates a 503 Service Unavailable error for an expired license.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 503 <see cref="Error"/>.</returns>
    public static Error LicenseExpired(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.LicenseExpired,
            code,
            description,
            (int)HttpStatusCode.ServiceUnavailable,
            target);
    }

    /// <summary>
    /// Creates a 503 Service Unavailable error for a cancelled license.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="target">The target that caused the error, if applicable.</param>
    /// <returns>A 503 <see cref="Error"/>.</returns>
    public static Error LicenseCancelled(
        string code,
        string description,
        string? target = null)
    {
        return new Error(
            ErrorConstant.ErrorTypes.LicenseCancelled,
            code,
            description,
            (int)HttpStatusCode.ServiceUnavailable,
            target);
    }

    #endregion
}