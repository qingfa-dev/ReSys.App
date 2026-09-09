using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.Results;

public class ResultOperatorTest
{
    [Theory]
    [InlineData("Code1", "Desc1")]
    [InlineData("Test.Code", "Description")]
    public void ImplicitOperator_SingleError_ShouldCreateFailureResult(string code, string desc)
    {
        Error error = Error.BadRequest(code, desc);

        Result<Error> result = error;

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

        Result<Error> result = errors;

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

        Result<Error> result = errors;

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("Test.Code", "Not found")]
    [InlineData("Other.Code", "Other desc")]
    public void ImplicitOperator_SingleError_ShouldPreserveErrorDetails(string code, string desc)
    {
        var error = Error.NotFound(code, desc);

        Result<Error> result = error;

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
            Result<Error> result = errors;
        };

        act.Should().Throw<ArgumentException>();
    }
}