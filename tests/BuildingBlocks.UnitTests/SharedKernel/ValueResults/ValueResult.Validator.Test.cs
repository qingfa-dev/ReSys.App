using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.ValueResults;

public class ValueResultValidatorTest
{
    #region ValidateValueForSuccess

    [Theory]
    [InlineData("hello")]
    [InlineData("")]
    public void ValidateValueForSuccess_SuccessWithNonNull_ShouldNotThrow(string value)
    {
        Action act = () => ValueResultValidator.ValidateValueForSuccess(value, true);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateValueForSuccess_FailureWithNull_ShouldNotThrow()
    {
        Action act = () => ValueResultValidator.ValidateValueForSuccess<string>(null, false);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateValueForSuccess_SuccessWithNull_ShouldThrowArgumentException()
    {
        Action act = () => ValueResultValidator.ValidateValueForSuccess<string>(null, true);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"{ValueResultConstant.Codes.ValueRequired} : *");
    }

    #endregion

    #region ValidateValueForFailure

    [Fact]
    public void ValidateValueForFailure_FailureWithNull_ShouldNotThrow()
    {
        Action act = () => ValueResultValidator.ValidateValueForFailure<string>(null, false);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateValueForFailure_SuccessWithNonNull_ShouldNotThrow()
    {
        Action act = () => ValueResultValidator.ValidateValueForFailure("hello", true);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateValueForFailure_FailureWithNonNull_ShouldThrowArgumentException()
    {
        Action act = () => ValueResultValidator.ValidateValueForFailure("hello", false);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"{ValueResultConstant.Codes.ValueNotAllowed} : *");
    }

    #endregion

    #region ValidateStatusCode

    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(202)]
    [InlineData(204)]
    [InlineData(300)]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    [InlineData(599)]
    public void ValidateStatusCode_ValidStatus_ShouldNotThrow(int status)
    {
        Action act = () => ValueResultValidator.ValidateStatusCode(status);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(99)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(600)]
    [InlineData(999)]
    public void ValidateStatusCode_InvalidStatus_ShouldThrowArgumentOutOfRangeException(int status)
    {
        Action act = () => ValueResultValidator.ValidateStatusCode(status);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"{ValueResultConstant.Codes.InvalidStatusCode} : *");
    }

    #endregion

    #region ValidateSuccessStatus

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(202)]
    [InlineData(204)]
    public void ValidateSuccessStatus_SuccessWith2xx_ShouldNotThrow(int status)
    {
        Action act = () => ValueResultValidator.ValidateSuccessStatus(status, true);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    public void ValidateSuccessStatus_FailureWith4xx_ShouldNotThrow(int status)
    {
        Action act = () => ValueResultValidator.ValidateSuccessStatus(status, false);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    public void ValidateSuccessStatus_SuccessWithNon2xx_ShouldThrowArgumentOutOfRangeException(int status)
    {
        Action act = () => ValueResultValidator.ValidateSuccessStatus(status, true);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"{ValueResultConstant.Codes.InvalidSuccessStatus} : *");
    }

    #endregion

    #region ValidateFailureStatus

    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    public void ValidateFailureStatus_FailureWith4xx5xx_ShouldNotThrow(int status)
    {
        Action act = () => ValueResultValidator.ValidateFailureStatus(status, false);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    public void ValidateFailureStatus_SuccessWith2xx_ShouldNotThrow(int status)
    {
        Action act = () => ValueResultValidator.ValidateFailureStatus(status, true);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(300)]
    public void ValidateFailureStatus_FailureWithNon4xx5xx_ShouldThrowArgumentOutOfRangeException(int status)
    {
        Action act = () => ValueResultValidator.ValidateFailureStatus(status, false);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"{ValueResultConstant.Codes.InvalidFailureStatus} : *");
    }

    #endregion

    #region ValidateResult

    [Theory]
    [InlineData("hello", true, StatusCodes.Status200OK)]
    [InlineData("hello", true, StatusCodes.Status201Created)]
    [InlineData(null, false, StatusCodes.Status400BadRequest)]
    [InlineData(null, false, StatusCodes.Status404NotFound)]
    public void ValidateResult_ValidState_ShouldNotThrow(string? value, bool isSuccess, int status)
    {
        Action act = () => ValueResultValidator.ValidateResult(value, isSuccess, status);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateResult_SuccessWithNullValue_ShouldThrowArgumentException()
    {
        Action act = () => ValueResultValidator.ValidateResult<string>(null, true, 200);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"{ValueResultConstant.Codes.ValueRequired} : *");
    }

    [Theory]
    [InlineData(99)]
    [InlineData(600)]
    public void ValidateResult_InvalidStatusCode_ShouldThrowArgumentOutOfRangeException(int status)
    {
        Action act = () => ValueResultValidator.ValidateResult("hello", true, status);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion
}