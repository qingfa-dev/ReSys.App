using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.Results;

public class ResultTest
{
    [Theory]
    [InlineData(true, StatusCodes.Status200OK)]
    [InlineData(true, StatusCodes.Status201Created)]
    [InlineData(false, StatusCodes.Status400BadRequest)]
    [InlineData(false, StatusCodes.Status404NotFound)]
    public void Create_ShouldHaveCorrectProperties(bool isSuccess, int status)
    {
        var errors = isSuccess
            ? []
            : new List<Error> { Error.BadRequest("Code", "Desc") };

        var result = Result<Error>.Create(isSuccess, status, errors);

        result.IsSuccess.Should().Be(isSuccess);
        result.Status.Should().Be(status);
        result.Errors.Should().HaveCount(isSuccess ? 0 : 1);
        result.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithErrors_ShouldHaveCorrectProperties()
    {
        var errors = new List<Error> { Error.BadRequest("Code", "Desc") }.AsReadOnly();

        var result = Result<Error>.Create(false, StatusCodes.Status400BadRequest, errors);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_NullErrors_ShouldDefaultToEmpty()
    {
        var result = Result<Error>.Create(true, StatusCodes.Status200OK, null);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }


    [Theory]
    [InlineData(StatusCodes.Status200OK)]
    [InlineData(StatusCodes.Status201Created)]
    [InlineData(StatusCodes.Status202Accepted)]
    [InlineData(StatusCodes.Status204NoContent)]
    public void Success_ShouldHaveCorrectProperties(int status)
    {
        var result = Result<Error>.Success(status);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Status.Should().Be(status);
        result.Errors.Should().BeEmpty();
        result.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Success_Default_ShouldReturn200()
    {
        var result = Result<Error>.Success();

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status200OK);
    }

    [Theory]
    [InlineData("Code1", "Desc1")]
    [InlineData("Test.Code", "Description")]
    [InlineData("X", "Y")]
    public void Failure_SingleError_ShouldHaveCorrectProperties(string code, string desc)
    {
        var error = Error.BadRequest(code, desc);
        var result = Result<Error>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().BeEquivalentTo(new[] { error });
        result.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Failure_MultipleErrors_ShouldHaveCorrectProperties()
    {
        var error1 = Error.BadRequest("Code1", "Desc1");
        var error2 = Error.BadRequest("Code2", "Desc2");
        var result = Result<Error>.Failure(error1, error2);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Failure_IEnumerableErrors_ShouldHaveCorrectProperties()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(new List<Error> { error });

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_NullError_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<Error>.Failure((Error)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_NullErrorsArray_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<Error>.Failure((Error[])null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_NullErrorsEnumerable_ShouldThrowArgumentNullException()
    {
        Action act = () => Result<Error>.Failure((IEnumerable<Error>)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_EmptyArray_ShouldThrowArgumentException()
    {
        Action act = () => Result<Error>.Failure(Array.Empty<Error>());
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Failure_EmptyList_ShouldThrowArgumentException()
    {
        Action act = () => Result<Error>.Failure(new List<Error>());
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Failure_MixedStatusCodes_ShouldThrowArgumentException()
    {
        var error1 = Error.BadRequest("Code1", "Desc1");
        var error2 = Error.NotFound("Code2", "Desc2");

        Action act = () => Result<Error>.Failure(error1, error2);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Failure_ExceedsMaxErrors_ShouldThrowArgumentException()
    {
        var errors = Enumerable.Repeat(
            Error.BadRequest("Code", "Desc"),
            ResultConstant.Constraints.MaxErrors + 1).ToArray();

        Action act = () => Result<Error>.Failure(errors);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToString_Success_ShouldReturnSuccessString()
    {
        var result = Result<Error>.Ok();
        result.ToString().Should().Be($"Success({StatusCodes.Status200OK})");
    }

    [Fact]
    public void ToString_Failure_ShouldReturnFailureString()
    {
        var error = Error.BadRequest("Test.Code", "Desc");
        var result = Result<Error>.Failure(error);
        result.ToString().Should().Contain("Failure");
        result.ToString().Should().Contain($"{StatusCodes.Status400BadRequest}");
        result.ToString().Should().Contain("Test.Code");
    }

    [Fact]
    public void Result_IResult_ShouldImplementInterface()
    {
        var result = Result<Error>.Ok();
        result.Should().BeAssignableTo<IResult<Error>>();
    }

    [Fact]
    public void WithResultMeta_ShouldSetMetadata()
    {
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<Error>.Ok().WithResultMeta(metadata);

        result.Metadata.Should().ContainKey("key");
        result.Metadata["key"].Should().Be("value");
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void WithResultMeta_OnFailure_ShouldSetMetadata()
    {
        var error = Error.BadRequest("Code", "Desc");
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<Error>.Failure(error).WithResultMeta(metadata);

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
        var result = Result<Error>.Created().WithResultMeta(metadata);

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
        var result = Result<Error>.Accepted().WithResultMeta(metadata);

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
        var result = Result<Error>.NoContent().WithResultMeta(metadata);

        result.Status.Should().Be(StatusCodes.Status204NoContent);
        result.Metadata.Should().ContainKey(key);
    }
}