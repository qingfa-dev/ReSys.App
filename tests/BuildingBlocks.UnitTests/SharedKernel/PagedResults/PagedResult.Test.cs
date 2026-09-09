using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedResultTest
{
    #region Success Factories

    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        var items = new[] { "a", "b", "c" };

        var result = PagedResult<string, Error>.Success(items, 1, 10, 30);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(items);
        result.PagedMetadata.PageNumber.Should().Be(1);
        result.PagedMetadata.TotalPages.Should().Be(3);
        result.PagedMetadata.ItemCount.Should().Be(10);
        result.PagedMetadata.TotalCount.Should().Be(30);
    }

    [Fact]
    public void Success_WithCustomStatus_ShouldSetStatus()
    {
        var items = new[] { 1, 2, 3 };

        var result = PagedResult<int, Error>.Success(items, 1, 10, 30, StatusCodes.Status202Accepted);

        result.Status.Should().Be(StatusCodes.Status202Accepted);
    }

    [Fact]
    public void Ok_ShouldCreate200Result()
    {
        var items = new[] { 1 };

        var result = PagedResult<int, Error>.Ok(items, 1, 10, 5);

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public void Created_ShouldCreate201Result()
    {
        var items = new[] { 1 };

        var result = PagedResult<int, Error>.Created(items, 1, 10, 5);

        result.Status.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public void Accepted_ShouldCreate202Result()
    {
        var items = new[] { 1 };

        var result = PagedResult<int, Error>.Accepted(items, 1, 10, 5);

        result.Status.Should().Be(StatusCodes.Status202Accepted);
    }

    #endregion

    #region Failure Factories

    [Fact]
    public void Failure_WithSingleError_ShouldCreateFailureResult()
    {
        var error = Error.NotFound("test.notfound", "Not found");

        var result = PagedResult<string, Error>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(error.Status);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_WithParams_ShouldCreateFailureResult()
    {
        var error = Error.BadRequest("test.bad", "Bad request");

        var result = PagedResult<string, Error>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_WithParamsArray_ShouldCreateFailureResult()
    {
        var error = Error.BadRequest("test.bad", "Bad request");

        var result = PagedResult<string, Error>.Failure(new Error[] { error });

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_WithPagingParams_ShouldCreateFailureResult()
    {
        var error = Error.NotFound("test.notfound", "Not found");

        var result = PagedResult<string, Error>.Failure(3, 10, 50, error);

        result.IsSuccess.Should().BeFalse();
        result.PagedMetadata.PageNumber.Should().Be(3);
    }

    #endregion

    #region Create

    [Fact]
    public void Create_ShouldCreatePagedResult()
    {
        var items = new[] { 1, 2 };

        var result = PagedResult<int, Error>.Create(
            items, true, StatusCodes.Status200OK, null, 2, 10, 25);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(items);
        result.PagedMetadata.PageNumber.Should().Be(2);
        result.PagedMetadata.TotalPages.Should().Be(3);
    }

    #endregion

    #region Properties

    [Fact]
    public void Error_ShouldReturnFirstErrorOnFailure()
    {
        var error = Error.BadRequest("test.bad", "Bad request");

        var result = PagedResult<string, Error>.Failure(error);

        result.Errors.Should().BeEquivalentTo(new[] { error });
    }

    [Fact]
    public void Error_ShouldReturnDefaultOnSuccess()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 1);

        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Metadata_ShouldBeDictionary()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 10);

        result.Metadata.Should().NotBeNull();
        result.Metadata["pageNumber"].Should().Be(1);
    }

    [Fact]
    public void PagedMetadata_ViaInterface_ShouldReturnSameMetadata()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 10);

        IPagedResult<string, Error> iface = result;

        iface.PagedMetadata.PageNumber.Should().Be(1);
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_Success_ShouldFormatCorrectly()
    {
        var result = PagedResult<string, Error>.Success(["a", "b"], 2, 10, 25);

        var str = result.ToString();

        str.Should().Contain("Success");
        str.Should().Contain("2");
        str.Should().Contain("3");
    }

    [Fact]
    public void ToString_Failure_ShouldFormatCorrectly()
    {
        var error = Error.NotFound("test.notfound", "Not found");
        var result = PagedResult<string, Error>.Failure(error);

        var str = result.ToString();

        str.Should().Contain("Failure");
        str.Should().Contain(error.Status.ToString());
    }

    #endregion
}