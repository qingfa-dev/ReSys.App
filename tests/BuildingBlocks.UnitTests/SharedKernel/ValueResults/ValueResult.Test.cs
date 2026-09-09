using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.ValueResults;

public class ValueResultTest
{
    [Theory]
    [InlineData("hello", true, StatusCodes.Status200OK)]
    [InlineData("hello", true, StatusCodes.Status201Created)]
    [InlineData(null, false, StatusCodes.Status400BadRequest)]
    [InlineData(null, false, StatusCodes.Status404NotFound)]
    public void Create_ShouldHaveCorrectProperties(string? value, bool isSuccess, int status)
    {
        var errors = isSuccess
            ? []
            : new List<Error> { Error.BadRequest("Code", "Desc") };

        var result = Result<string, Error>.Create(value, isSuccess, status, errors);

        result.IsSuccess.Should().Be(isSuccess);
        result.Status.Should().Be(status);
        result.Value.Should().Be(value);
        result.Errors.Should().HaveCount(isSuccess ? 0 : 1);
        result.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithErrors_ShouldHaveCorrectProperties()
    {
        var errors = new List<Error> { Error.BadRequest("Code", "Desc") };

        var result = Result<string, Error>.Create(null, false, StatusCodes.Status400BadRequest, errors);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(1);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Create_NullErrors_ShouldDefaultToEmpty()
    {
        var result = Result<string, Error>.Create("hello", true, StatusCodes.Status200OK, null);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("hello");
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("hello", StatusCodes.Status200OK)]
    [InlineData("hello", StatusCodes.Status201Created)]
    [InlineData("hello", StatusCodes.Status202Accepted)]
    [InlineData("hello", StatusCodes.Status204NoContent)]
    public void Success_ShouldHaveCorrectProperties(string value, int status)
    {
        var result = Result<string, Error>.Success(value, status);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Status.Should().Be(status);
        result.Value.Should().Be(value);
        result.HasValue.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Success_NullValue_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.Success(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ok_ShouldReturn200()
    {
        var result = Result<string, Error>.Ok("hello");

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void Created_ShouldReturn201()
    {
        var result = Result<string, Error>.Created("hello");

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void Created_NullValue_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.Created(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Accepted_ShouldReturn202()
    {
        var result = Result<string, Error>.Accepted("hello");

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status202Accepted);
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void Accepted_NullValue_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.Accepted(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NoContent_ShouldReturn204()
    {
        var result = Result<string, Error>.NoContent("hello");

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status204NoContent);
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void NoContent_NullValue_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.NoContent(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_SingleError_ShouldHaveCorrectProperties()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<string, Error>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeNull();
        result.HasValue.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().BeEquivalentTo(new[] { error });
        result.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Failure_MultipleErrors_ShouldHaveCorrectProperties()
    {
        var error1 = Error.BadRequest("Code1", "Desc1");
        var error2 = Error.BadRequest("Code2", "Desc2");
        var result = Result<string, Error>.Failure(error1, error2);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(2);
        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Failure_IEnumerableErrors_ShouldHaveCorrectProperties()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<string, Error>.Failure(new List<Error> { error });

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_NullError_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.Failure((Error)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_NullErrorsArray_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.Failure((Error[])null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_NullErrorsEnumerable_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<string, Error>.Failure((IEnumerable<Error>)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_EmptyArray_ShouldThrowArgumentException()
    {
        Action act = () => Result<string, Error>.Failure(Array.Empty<Error>());
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Failure_EmptyList_ShouldThrowArgumentException()
    {
        Action act = () => Result<string, Error>.Failure(new List<Error>());
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Failure_MixedStatusCodes_ShouldThrowArgumentException()
    {
        var error1 = Error.BadRequest("Code1", "Desc1");
        var error2 = Error.NotFound("Code2", "Desc2");

        Action act = () => Result<string, Error>.Failure(error1, error2);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_Value_ShouldCreateSuccessResult()
    {
        var result = Result<string, Error>.From("hello");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void From_Error_ShouldCreateFailureResult()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<string, Error>.From(error);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] { error });
    }

    [Fact]
    public void ToString_Success_ShouldIncludeValue()
    {
        var result = Result<string, Error>.Ok("hello");
        result.ToString().Should().Contain("Success");
        result.ToString().Should().Contain("hello");
    }

    [Fact]
    public void ToString_Failure_ShouldIncludeErrorCode()
    {
        var error = Error.BadRequest("Test.Code", "Desc");
        var result = Result<string, Error>.Failure(error);
        result.ToString().Should().Contain("Failure");
        result.ToString().Should().Contain("Test.Code");
    }

    [Fact]
    public void ValueResult_IResult_ShouldImplementInterface()
    {
        var result = Result<string, Error>.Ok("hello");
        result.Should().BeAssignableTo<IResult<string, Error>>();
        result.Should().BeAssignableTo<IValueOf<string>>();
    }

    [Fact]
    public void WithResultMeta_ShouldSetMetadata()
    {
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<string, Error>.Ok("hello").WithResultMeta(metadata);

        result.Metadata.Should().ContainKey("key");
        result.Metadata["key"].Should().Be("value");
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void WithResultMeta_OnFailure_ShouldSetMetadata()
    {
        var error = Error.BadRequest("Code", "Desc");
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<string, Error>.Failure(error).WithResultMeta(metadata);

        result.Metadata.Should().ContainKey("key");
        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData("key1", "value1")]
    [InlineData("key2", 42)]
    [InlineData("key3", null)]
    public void Created_WithMetadata_ShouldContainMetadata(string key, object? value)
    {
        var metadata = new Dictionary<string, object?> { [key] = value };
        var result = Result<string, Error>.Created("hello").WithResultMeta(metadata);

        result.Status.Should().Be(StatusCodes.Status201Created);
        result.Metadata.Should().ContainKey(key);
    }

    [Theory]
    [InlineData("key1", "value1")]
    [InlineData("key2", 42)]
    [InlineData("key3", null)]
    public void Accepted_WithMetadata_ShouldContainMetadata(string key, object? value)
    {
        var metadata = new Dictionary<string, object?> { [key] = value };
        var result = Result<string, Error>.Accepted("hello").WithResultMeta(metadata);

        result.Status.Should().Be(StatusCodes.Status202Accepted);
        result.Metadata.Should().ContainKey(key);
    }

    [Theory]
    [InlineData("key1", "value1")]
    [InlineData("key2", 42)]
    [InlineData("key3", null)]
    public void NoContent_WithMetadata_ShouldContainMetadata(string key, object? value)
    {
        var metadata = new Dictionary<string, object?> { [key] = value };
        var result = Result<string, Error>.NoContent("hello").WithResultMeta(metadata);

        result.Status.Should().Be(StatusCodes.Status204NoContent);
        result.Metadata.Should().ContainKey(key);
    }
}
