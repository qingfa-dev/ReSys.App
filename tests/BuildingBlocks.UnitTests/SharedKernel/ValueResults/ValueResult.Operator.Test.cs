using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.ValueResults;

public class ValueResultOperatorTest
{
    [Theory]
    [InlineData("hello")]
    [InlineData("")]
    public void ImplicitOperator_Value_ShouldCreateSuccessResult(string value)
    {
        Result<string, Error> result = value;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
        result.Status.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void ImplicitOperator_Error_ShouldCreateFailureResult()
    {
        Error error = Error.BadRequest("Code", "Desc");

        Result<string, Error> result = error;

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().BeEquivalentTo(new[] { error });
    }

    [Fact]
    public void ImplicitOperator_ErrorArray_ShouldCreateFailureResult()
    {
        Error[] errors =
        [
            Error.BadRequest("Code1", "Desc1"),
            Error.BadRequest("Code2", "Desc2")
        ];

        Result<string, Error> result = errors;

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void ImplicitOperator_ListErrors_ShouldCreateFailureResult()
    {
        List<Error> errors =
        [
            Error.BadRequest("Code1", "Desc1"),
            Error.BadRequest("Code2", "Desc2")
        ];

        Result<string, Error> result = errors;

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("Test.Code", "Not found")]
    [InlineData("Other.Code", "Other desc")]
    public void ImplicitOperator_Error_ShouldPreserveErrorDetails(string code, string desc)
    {
        var error = Error.NotFound(code, desc);

        Result<string, Error> result = error;

        result.Errors[0].Code.Should().Be(code);
        result.Errors[0].Description.Should().Be(desc);
    }

    [Fact]
    public void ImplicitOperator_ErrorArray_MixedErrors_ShouldReject()
    {
        Error[] errors =
        [
            Error.BadRequest("Code1", "Desc1"),
            Error.NotFound("Code2", "Desc2")
        ];

        Action act = () =>
        {
            Result<string, Error> result = errors;
        };

        act.Should().Throw<ArgumentException>();
    }
}