namespace BuildingBlocks.UnitTests.SharedKernel.Models;

public class ErrorMethodTest
{
    #region Server Errors

    [Fact]
    public void Unexpected_DefaultParameters_ShouldReturnServerErrorDefaults()
    {
        var error = Error.Unexpected();

        error.Status.Should().Be(500);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.ServerError);
        error.Code.Should().Be(ErrorConstant.Defaults.Code);
        error.Description.Should().Be(ErrorConstant.Defaults.Message);
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Unexpected_CustomCodeAndDescription_ShouldReturnCustomValues()
    {
        var error = Error.Unexpected("Custom.Code", "Custom message.");

        error.Status.Should().Be(500);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.ServerError);
        error.Code.Should().Be("Custom.Code");
        error.Description.Should().Be("Custom message.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Unexpected_WithTarget_ShouldReturnTarget()
    {
        var error = Error.Unexpected(target: "service");

        error.Target.Should().Be("service");
    }

    [Fact]
    public void ServerError_DefaultParameters_ShouldReturnServerErrorDefaults()
    {
        var error = Error.ServerError();

        error.Status.Should().Be(500);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.ServerError);
        error.Code.Should().Be(ErrorConstant.Defaults.Code);
        error.Description.Should().Be(ErrorConstant.Defaults.Message);
        error.Target.Should().BeNull();
    }

    [Fact]
    public void ServerError_CustomCodeAndDescription_ShouldReturnCustomValues()
    {
        var error = Error.ServerError("Custom.Code", "Custom message.");

        error.Status.Should().Be(500);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.ServerError);
        error.Code.Should().Be("Custom.Code");
        error.Description.Should().Be("Custom message.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void ServerError_WithTarget_ShouldReturnTarget()
    {
        var error = Error.ServerError(target: "service");

        error.Target.Should().Be("service");
    }

    [Fact]
    public void Failure_DefaultParameters_ShouldReturnFailureDefaults()
    {
        var error = Error.Failure();

        error.Status.Should().Be(500);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.Failure);
        error.Code.Should().Be(ErrorConstant.Codes.Failure);
        error.Description.Should().Be(ErrorConstant.Messages.Failure);
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Failure_CustomCodeAndDescription_ShouldReturnCustomValues()
    {
        var error = Error.Failure("Custom.Code", "Custom message.");

        error.Status.Should().Be(500);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.Failure);
        error.Code.Should().Be("Custom.Code");
        error.Description.Should().Be("Custom message.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Failure_WithTarget_ShouldReturnTarget()
    {
        var error = Error.Failure(target: "service");

        error.Target.Should().Be("service");
    }

    #endregion

    #region Client Errors (4xx)

    [Fact]
    public void BadRequest_ShouldReturn400BadRequest()
    {
        var error = Error.BadRequest("Test.Code", "Test description.");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.BadRequest);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void BadRequest_WithTarget_ShouldReturnTarget()
    {
        var error = Error.BadRequest("Test.Code", "Desc", "field");

        error.Target.Should().Be("field");
    }

    [Fact]
    public void Unauthorized_ShouldReturn401Unauthorized()
    {
        var error = Error.Unauthorized("Test.Code", "Test description.");

        error.Status.Should().Be(401);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.Unauthorized);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Unauthorized_WithTarget_ShouldReturnTarget()
    {
        var error = Error.Unauthorized("Test.Code", "Desc", "header");

        error.Target.Should().Be("header");
    }

    [Fact]
    public void Forbidden_ShouldReturn403Forbidden()
    {
        var error = Error.Forbidden("Test.Code", "Test description.");

        error.Status.Should().Be(403);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.Forbidden);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Forbidden_WithTarget_ShouldReturnTarget()
    {
        var error = Error.Forbidden("Test.Code", "Desc", "resource");

        error.Target.Should().Be("resource");
    }

    [Fact]
    public void NotFound_ShouldReturn404NotFound()
    {
        var error = Error.NotFound("Test.Code", "Test description.");

        error.Status.Should().Be(404);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.NotFound);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void NotFound_WithTarget_ShouldReturnTarget()
    {
        var error = Error.NotFound("Test.Code", "Desc", "endpoint");

        error.Target.Should().Be("endpoint");
    }

    [Fact]
    public void AlreadyExists_ShouldReturn409Conflict()
    {
        var error = Error.AlreadyExists("Test.Code", "Test description.");

        error.Status.Should().Be(409);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.AlreadyExists);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void AlreadyExists_WithTarget_ShouldReturnTarget()
    {
        var error = Error.AlreadyExists("Test.Code", "Desc", "resource");

        error.Target.Should().Be("resource");
    }

    [Fact]
    public void Validation_ShouldReturn422UnprocessableEntity()
    {
        var error = Error.Validation("Test.Code", "Test description.");

        error.Status.Should().Be(422);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.ValidationError);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Validation_WithTarget_ShouldReturnTarget()
    {
        var error = Error.Validation("Test.Code", "Desc", "model");

        error.Target.Should().Be("model");
    }

    [Fact]
    public void BusinessRuleViolation_ShouldReturn422UnprocessableEntity()
    {
        var error = Error.BusinessRuleViolation("Test.Code", "Test description.");

        error.Status.Should().Be(422);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.BusinessRuleViolation);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void BusinessRuleViolation_WithTarget_ShouldReturnTarget()
    {
        var error = Error.BusinessRuleViolation("Test.Code", "Desc", "rule");

        error.Target.Should().Be("rule");
    }

    #endregion

    #region Request Validation Errors (400)

    [Fact]
    public void MissingBodyProperty_ShouldReturn400BadRequest()
    {
        var error = Error.MissingBodyProperty("name");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.MissingBodyProperty);
        error.Code.Should().Be("Request.MissingBodyProperty");
        error.Description.Should().Contain("name");
        error.Description.Should().Contain("required");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void MissingBodyProperty_WithTarget_ShouldReturnTarget()
    {
        var error = Error.MissingBodyProperty("name", "body");

        error.Target.Should().Be("body");
    }

    [Fact]
    public void MissingBodyProperty_Null_ShouldThrow()
    {
        Action act = () => Error.MissingBodyProperty(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MissingBodyProperty_Whitespace_ShouldThrow()
    {
        Action act = () => Error.MissingBodyProperty("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MissingRequestHeader_ShouldReturn400BadRequest()
    {
        var error = Error.MissingRequestHeader("X-Custom");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.MissingRequestHeader);
        error.Code.Should().Be("Request.MissingHeader");
        error.Description.Should().Contain("X-Custom");
        error.Description.Should().Contain("required");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void MissingRequestHeader_WithTarget_ShouldReturnTarget()
    {
        var error = Error.MissingRequestHeader("X-Custom", "header");

        error.Target.Should().Be("header");
    }

    [Fact]
    public void MissingRequestHeader_Null_ShouldThrow()
    {
        Action act = () => Error.MissingRequestHeader(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MissingRequestHeader_Whitespace_ShouldThrow()
    {
        Action act = () => Error.MissingRequestHeader("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MissingRequestParameter_ShouldReturn400BadRequest()
    {
        var error = Error.MissingRequestParameter("id");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.MissingRequestParameter);
        error.Code.Should().Be("Request.MissingParameter");
        error.Description.Should().Contain("id");
        error.Description.Should().Contain("required");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void MissingRequestParameter_WithTarget_ShouldReturnTarget()
    {
        var error = Error.MissingRequestParameter("id", "query");

        error.Target.Should().Be("query");
    }

    [Fact]
    public void MissingRequestParameter_Null_ShouldThrow()
    {
        Action act = () => Error.MissingRequestParameter(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MissingRequestParameter_Whitespace_ShouldThrow()
    {
        Action act = () => Error.MissingRequestParameter("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidBodyPropertyFormat_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidBodyPropertyFormat("email");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidBodyPropertyFormat);
        error.Code.Should().Be("Request.InvalidBodyPropertyFormat");
        error.Description.Should().Contain("email");
        error.Description.Should().Contain("invalid format");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidBodyPropertyFormat_WithTarget_ShouldReturnTarget()
    {
        var error = Error.InvalidBodyPropertyFormat("email", "body");

        error.Target.Should().Be("body");
    }

    [Fact]
    public void InvalidBodyPropertyFormat_Null_ShouldThrow()
    {
        Action act = () => Error.InvalidBodyPropertyFormat(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidBodyPropertyFormat_Whitespace_ShouldThrow()
    {
        Action act = () => Error.InvalidBodyPropertyFormat("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidBodyPropertyValue_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidBodyPropertyValue("age");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidBodyPropertyValue);
        error.Code.Should().Be("Request.InvalidBodyPropertyValue");
        error.Description.Should().Contain("age");
        error.Description.Should().Contain("invalid value");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidBodyPropertyValue_WithTarget_ShouldReturnTarget()
    {
        var error = Error.InvalidBodyPropertyValue("age", "body");

        error.Target.Should().Be("body");
    }

    [Fact]
    public void InvalidBodyPropertyValue_Null_ShouldThrow()
    {
        Action act = () => Error.InvalidBodyPropertyValue(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidBodyPropertyValue_Whitespace_ShouldThrow()
    {
        Action act = () => Error.InvalidBodyPropertyValue("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidRequestParameterFormat_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidRequestParameterFormat("page");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidRequestParameterFormat);
        error.Code.Should().Be("Request.InvalidParameterFormat");
        error.Description.Should().Contain("page");
        error.Description.Should().Contain("invalid format");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidRequestParameterFormat_WithTarget_ShouldReturnTarget()
    {
        var error = Error.InvalidRequestParameterFormat("page", "query");

        error.Target.Should().Be("query");
    }

    [Fact]
    public void InvalidRequestParameterFormat_Null_ShouldThrow()
    {
        Action act = () => Error.InvalidRequestParameterFormat(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidRequestParameterFormat_Whitespace_ShouldThrow()
    {
        Action act = () => Error.InvalidRequestParameterFormat("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidRequestParameterValue_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidRequestParameterValue("sort");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidRequestParameterValue);
        error.Code.Should().Be("Request.InvalidParameterValue");
        error.Description.Should().Contain("sort");
        error.Description.Should().Contain("invalid value");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidRequestParameterValue_WithTarget_ShouldReturnTarget()
    {
        var error = Error.InvalidRequestParameterValue("sort", "query");

        error.Target.Should().Be("query");
    }

    [Fact]
    public void InvalidRequestParameterValue_Null_ShouldThrow()
    {
        Action act = () => Error.InvalidRequestParameterValue(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidRequestParameterValue_Whitespace_ShouldThrow()
    {
        Action act = () => Error.InvalidRequestParameterValue("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidRequestHeaderFormat_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidRequestHeaderFormat("Accept");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidRequestHeaderFormat);
        error.Code.Should().Be("Request.InvalidHeaderFormat");
        error.Description.Should().Contain("Accept");
        error.Description.Should().Contain("invalid format");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidRequestHeaderFormat_WithTarget_ShouldReturnTarget()
    {
        var error = Error.InvalidRequestHeaderFormat("Accept", "header");

        error.Target.Should().Be("header");
    }

    [Fact]
    public void InvalidRequestHeaderFormat_Null_ShouldThrow()
    {
        Action act = () => Error.InvalidRequestHeaderFormat(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidRequestHeaderFormat_Whitespace_ShouldThrow()
    {
        Action act = () => Error.InvalidRequestHeaderFormat("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InvalidParameters_DefaultCode_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidParameters("Invalid query.");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidParameters);
        error.Code.Should().Be("Request.InvalidParameters");
        error.Description.Should().Be("Invalid query.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidParameters_CustomCode_ShouldReturn400BadRequest()
    {
        var error = Error.InvalidParameters("Invalid query.", "Custom.Code");

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.InvalidParameters);
        error.Code.Should().Be("Custom.Code");
        error.Description.Should().Be("Invalid query.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void InvalidParameters_WithTarget_ShouldReturnTarget()
    {
        var error = Error.InvalidParameters("Invalid query.", target: "query");

        error.Target.Should().Be("query");
    }

    #endregion

    #region Service Errors (5xx)

    [Fact]
    public void ServiceUnavailable_ShouldReturn503ServiceUnavailable()
    {
        var error = Error.ServiceUnavailable("Test.Code", "Test description.");

        error.Status.Should().Be(503);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.ServiceUnavailable);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void ServiceUnavailable_WithTarget_ShouldReturnTarget()
    {
        var error = Error.ServiceUnavailable("Test.Code", "Desc", "service");

        error.Target.Should().Be("service");
    }

    #endregion

    #region License Errors

    [Fact]
    public void LicenseExpired_ShouldReturn503ServiceUnavailable()
    {
        var error = Error.LicenseExpired("Test.Code", "Test description.");

        error.Status.Should().Be(503);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.LicenseExpired);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void LicenseExpired_WithTarget_ShouldReturnTarget()
    {
        var error = Error.LicenseExpired("Test.Code", "Desc", "license");

        error.Target.Should().Be("license");
    }

    [Fact]
    public void LicenseCancelled_ShouldReturn503ServiceUnavailable()
    {
        var error = Error.LicenseCancelled("Test.Code", "Test description.");

        error.Status.Should().Be(503);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.LicenseCancelled);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("Test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void LicenseCancelled_WithTarget_ShouldReturnTarget()
    {
        var error = Error.LicenseCancelled("Test.Code", "Desc", "license");

        error.Target.Should().Be("license");
    }

    #endregion

    #region Interface

    [Theory]
    [MemberData(nameof(AllFactoryMethods))]
    public void AllFactoryMethods_ShouldImplementIError(Func<IError> factory)
    {
        var error = factory();
        error.Should().BeAssignableTo<IError>();
    }

    public static IEnumerable<object[]> AllFactoryMethods =>
    [
        [new Func<IError>(() => Error.Unexpected())],
        [new Func<IError>(() => Error.Unexpected("C", "D"))],
        [new Func<IError>(() => Error.Unexpected("C", "D", "T"))],
        [new Func<IError>(() => Error.ServerError())],
        [new Func<IError>(() => Error.ServerError("C", "D"))],
        [new Func<IError>(() => Error.ServerError("C", "D", "T"))],
        [new Func<IError>(() => Error.Failure())],
        [new Func<IError>(() => Error.Failure("C", "D"))],
        [new Func<IError>(() => Error.Failure("C", "D", "T"))],
        [new Func<IError>(() => Error.BadRequest("C", "D"))],
        [new Func<IError>(() => Error.BadRequest("C", "D", "T"))],
        [new Func<IError>(() => Error.Unauthorized("C", "D"))],
        [new Func<IError>(() => Error.Unauthorized("C", "D", "T"))],
        [new Func<IError>(() => Error.Forbidden("C", "D"))],
        [new Func<IError>(() => Error.Forbidden("C", "D", "T"))],
        [new Func<IError>(() => Error.NotFound("C", "D"))],
        [new Func<IError>(() => Error.NotFound("C", "D", "T"))],
        [new Func<IError>(() => Error.AlreadyExists("C", "D"))],
        [new Func<IError>(() => Error.AlreadyExists("C", "D", "T"))],
        [new Func<IError>(() => Error.Validation("C", "D"))],
        [new Func<IError>(() => Error.Validation("C", "D", "T"))],
        [new Func<IError>(() => Error.BusinessRuleViolation("C", "D"))],
        [new Func<IError>(() => Error.BusinessRuleViolation("C", "D", "T"))],
        [new Func<IError>(() => Error.MissingBodyProperty("p"))],
        [new Func<IError>(() => Error.MissingBodyProperty("p", "T"))],
        [new Func<IError>(() => Error.MissingRequestHeader("h"))],
        [new Func<IError>(() => Error.MissingRequestHeader("h", "T"))],
        [new Func<IError>(() => Error.MissingRequestParameter("p"))],
        [new Func<IError>(() => Error.MissingRequestParameter("p", "T"))],
        [new Func<IError>(() => Error.InvalidBodyPropertyFormat("p"))],
        [new Func<IError>(() => Error.InvalidBodyPropertyFormat("p", "T"))],
        [new Func<IError>(() => Error.InvalidBodyPropertyValue("p"))],
        [new Func<IError>(() => Error.InvalidBodyPropertyValue("p", "T"))],
        [new Func<IError>(() => Error.InvalidRequestParameterFormat("p"))],
        [new Func<IError>(() => Error.InvalidRequestParameterFormat("p", "T"))],
        [new Func<IError>(() => Error.InvalidRequestParameterValue("p"))],
        [new Func<IError>(() => Error.InvalidRequestParameterValue("p", "T"))],
        [new Func<IError>(() => Error.InvalidRequestHeaderFormat("h"))],
        [new Func<IError>(() => Error.InvalidRequestHeaderFormat("h", "T"))],
        [new Func<IError>(() => Error.InvalidParameters("D"))],
        [new Func<IError>(() => Error.InvalidParameters("D", "C"))],
        [new Func<IError>(() => Error.InvalidParameters("D", "C", "T"))],
        [new Func<IError>(() => Error.ServiceUnavailable("C", "D"))],
        [new Func<IError>(() => Error.ServiceUnavailable("C", "D", "T"))],
        [new Func<IError>(() => Error.LicenseExpired("C", "D"))],
        [new Func<IError>(() => Error.LicenseExpired("C", "D", "T"))],
        [new Func<IError>(() => Error.LicenseCancelled("C", "D"))],
        [new Func<IError>(() => Error.LicenseCancelled("C", "D", "T"))],
    ];

    #endregion
}
