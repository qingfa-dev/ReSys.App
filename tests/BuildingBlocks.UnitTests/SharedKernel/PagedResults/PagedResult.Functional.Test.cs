namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedResultFunctionalTest
{
    #region WithPagedMetadata

    [Fact]
    public void WithPagedMetadata_ShouldReturnNewInstance()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 10);
        var newMetadata = PagedMetadata.From(3, 20, 50);

        var updated = result.WithPagedMetadata(newMetadata);

        updated.PagedMetadata.PageNumber.Should().Be(3);
        result.PagedMetadata.PageNumber.Should().Be(1);
    }

    #endregion

    #region WithPage

    [Fact]
    public void WithPage_ShouldUpdatePageNumber()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        var updated = result.WithPage(5);

        updated.PagedMetadata.PageNumber.Should().Be(5);
    }

    #endregion

    #region WithPageSize

    [Fact]
    public void WithPageSize_ShouldUpdatePageSize()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        var updated = result.WithPageSize(25);

        updated.PagedMetadata.PageSize.Should().Be(25);
    }

    #endregion

    #region WithKey

    [Fact]
    public void WithKey_ShouldAddCustomKey()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        var updated = result.WithKey("sortBy", "name");

        ((IReadOnlyDictionary<string, object?>)updated.PagedMetadata)["sortBy"].Should().Be("name");
    }

    [Fact]
    public void WithKey_ShouldNotMutateOriginal()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        _ = result.WithKey("sortBy", "name");

        result.PagedMetadata.ContainsKey("sortBy").Should().BeFalse();
    }

    #endregion

    #region NextPage

    [Fact]
    public void NextPage_WhenHasNext_ShouldMoveToNextPage()
    {
        var result = PagedResult<string, Error>.Success(["a"], 2, 10, 100);

        var next = result.NextPage();

        next.PagedMetadata.PageNumber.Should().Be(3);
    }

    [Fact]
    public void NextPage_WhenLastPage_ShouldStayOnSamePage()
    {
        var result = PagedResult<string, Error>.Success(["a"], 10, 10, 100);

        var next = result.NextPage();

        next.PagedMetadata.PageNumber.Should().Be(10);
    }

    #endregion

    #region PreviousPage

    [Fact]
    public void PreviousPage_WhenHasPrevious_ShouldMoveToPreviousPage()
    {
        var result = PagedResult<string, Error>.Success(["a"], 5, 10, 100);

        var prev = result.PreviousPage();

        prev.PagedMetadata.PageNumber.Should().Be(4);
    }

    [Fact]
    public void PreviousPage_WhenFirstPage_ShouldStayOnSamePage()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        var prev = result.PreviousPage();

        prev.PagedMetadata.PageNumber.Should().Be(1);
    }

    #endregion

    #region FirstPage

    [Fact]
    public void FirstPage_ShouldMoveToPage1()
    {
        var result = PagedResult<string, Error>.Success(["a"], 5, 10, 100);

        var first = result.FirstPage();

        first.PagedMetadata.PageNumber.Should().Be(1);
    }

    #endregion

    #region LastPage

    [Fact]
    public void LastPage_ShouldMoveToLastPage()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        var last = result.LastPage();

        last.PagedMetadata.PageNumber.Should().Be(10);
    }

    #endregion

    #region Fluent Chaining

    [Fact]
    public void FluentChaining_ShouldWork()
    {
        var result = PagedResult<string, Error>.Success(["a"], 1, 10, 100);

        var chained = result
            .WithPage(3)
            .WithPageSize(25)
            .WithKey("sortBy", "name")
            .NextPage();

        chained.PagedMetadata.PageNumber.Should().Be(4);
        chained.PagedMetadata.PageSize.Should().Be(25);
        ((IReadOnlyDictionary<string, object?>)chained.PagedMetadata)["sortBy"].Should().Be("name");
    }

    #endregion
}
