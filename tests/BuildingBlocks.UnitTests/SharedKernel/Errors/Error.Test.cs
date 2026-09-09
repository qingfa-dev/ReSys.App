using System.Text.Json;

using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.Errors;

public class ErrorTest
{
    [Fact]
    public void Error_Properties_ShouldBeCorrectlySet()
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
    public void Error_Properties_WithTarget_ShouldBeCorrectlySet()
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
    public void Error_Equality_SameValues_ShouldBeEqual()
    {
        var error1 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        var error2 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        error1.Should().Be(error2);
    }

    [Fact]
    public void Error_Equality_DifferentValues_ShouldNotBeEqual()
    {
        var error1 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Code.A",
            "Desc",
            StatusCodes.Status400BadRequest);

        var error2 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Code.B",
            "Desc",
            StatusCodes.Status400BadRequest);

        error1.Should().NotBe(error2);
    }

    [Fact]
    public void Error_Equality_DifferentTarget_ShouldNotBeEqual()
    {
        var error1 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Code",
            "Desc",
            StatusCodes.Status400BadRequest,
            "targetA");

        var error2 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Code",
            "Desc",
            StatusCodes.Status400BadRequest,
            "targetB");

        error1.Should().NotBe(error2);
    }

    [Fact]
    public void Error_GetHashCode_SameValues_ShouldMatch()
    {
        var error1 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        var error2 = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        error1.GetHashCode().Should().Be(error2.GetHashCode());
    }

    [Fact]
    public void Error_ToString_ShouldContainProperties()
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.NotFound,
            "Test.Code",
            "Desc",
            StatusCodes.Status404NotFound);

        var str = error.ToString();
        str.Should().Contain("404");
        str.Should().Contain("Test.Code");
        str.Should().Contain("Desc");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(600)]
    public void Error_Create_InvalidStatus_ShouldThrow(int status)
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Code",
            "Desc",
            status);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Error_Create_NullType_ShouldThrow()
    {
        Action act = () => Error.Create(
            null!,
            "Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Error_Create_InvalidCode_ShouldThrow(string? code)
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            code!,
            "Desc",
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Error_Create_InvalidDescription_ShouldThrow(string? description)
    {
        Action act = () => Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Code",
            description!,
            StatusCodes.Status400BadRequest);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Error_JsonSerialization_ShouldUseCorrectPropertyNames()
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest,
            "email");

        var json = JsonSerializer.Serialize(error);
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        root.GetProperty("status").GetInt32().Should().Be(400);
        root.GetProperty("type").GetString().Should().Be(ErrorConstant.ErrorTypes.BadRequest);
        root.GetProperty("code").GetString().Should().Be("Test.Code");
        root.GetProperty("description").GetString().Should().Be("Desc");
        root.GetProperty("target").GetString().Should().Be("email");
    }

    [Fact]
    public void Error_JsonSerialization_NullTarget_ShouldBeOmitted()
    {
        var error = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest);

        var json = JsonSerializer.Serialize(error);
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        root.TryGetProperty("target", out _).Should().BeFalse();
    }

    [Fact]
    public void Error_IError_Interface_ShouldExposeTarget()
    {
        IError error = Error.Create(
            ErrorConstant.ErrorTypes.BadRequest,
            "Test.Code",
            "Desc",
            StatusCodes.Status400BadRequest,
            "field");

        error.Target.Should().Be("field");
    }
}