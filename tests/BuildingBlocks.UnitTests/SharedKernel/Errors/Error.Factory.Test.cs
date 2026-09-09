using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.Errors;

public class ErrorFactoryTest
{
    [Fact]
    public void Create_ValidInputs_ShouldReturnErrorWithCorrectProperties()
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "A test description.",
            StatusCodes.Status400BadRequest);

        error.Status.Should().Be(400);
        error.Type.Should().Be(ErrorConstant.ErrorTypes.BadRequest);
        error.Code.Should().Be("Test.Code");
        error.Description.Should().Be("A test description.");
        error.Target.Should().BeNull();
    }

    [Fact]
    public void Create_WithTarget_ShouldReturnErrorWithTarget()
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "A test description.",
            StatusCodes.Status400BadRequest,
            "email");

        error.Target.Should().Be("email");
    }

    [Fact]
    public void Create_ShouldImplementIError()
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.NotFound,
            "Test.Code",
            "Desc",
            StatusCodes.Status404NotFound);

        error.Should().BeAssignableTo<IError>();
    }

    [Theory]
    [InlineData(200)]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    public void Create_VariousStatusCodes_ShouldSetStatusCorrectly(int status)
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            status);

        error.Status.Should().Be(status);
    }

    [Fact]
    public void Create_InvalidStatus_ShouldThrowArgumentOutOfRangeException()
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            99);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_StatusAboveRange_ShouldThrowArgumentOutOfRangeException()
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            600);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_NullType_ShouldThrowArgumentNullException()
    {
        Action act = () => Error.Create(
            null!,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_NullCode_ShouldThrowArgumentException()
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            null!,
            "Desc",
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_EmptyCode_ShouldThrowArgumentException()
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            string.Empty,
            "Desc",
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_CodeExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        var code = new string('a', ErrorConstant.Constraints.MaxCodeLength + 1);

        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            code,
            "Desc",
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_NullDescription_ShouldThrowArgumentException()
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            null!,
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_EmptyDescription_ShouldThrowArgumentException()
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            string.Empty,
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_DescriptionExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        var description = new string('b', ErrorConstant.Constraints.MaxDescriptionLength + 1);

        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            description,
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}