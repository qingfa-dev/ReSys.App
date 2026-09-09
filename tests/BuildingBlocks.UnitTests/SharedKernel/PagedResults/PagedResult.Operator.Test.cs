namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedResultOperatorTest
{
    #region List Implicit Operator

    [Fact]
    public void ImplicitOperator_List_ShouldCreateSuccessResult()
    {
        List<string> items = ["a", "b", "c"];

        PagedResult<string, Error> result = items;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(items);
        result.PagedMetadata.PageNumber.Should().Be(1);
        result.PagedMetadata.ItemCount.Should().Be(3);
        result.PagedMetadata.TotalCount.Should().Be(3);
    }

    #endregion

    #region Array Implicit Operator

    [Fact]
    public void ImplicitOperator_Array_ShouldCreateSuccessResult()
    {
        int[] items = [1, 2, 3];

        PagedResult<int, Error> result = items;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(items);
        result.PagedMetadata.PageNumber.Should().Be(1);
        result.PagedMetadata.ItemCount.Should().Be(3);
        result.PagedMetadata.TotalCount.Should().Be(3);
    }

    #endregion

    #region Error Implicit Operator

    [Fact]
    public void ImplicitOperator_Error_ShouldCreateFailureResult()
    {
        var error = Error.NotFound("test.notfound", "Not found");

        PagedResult<string, Error> result = error;

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    #endregion

    #region Error List Implicit Operator

    [Fact]
    public void ImplicitOperator_ErrorList_ShouldCreateFailureResult()
    {
        var errors = new List<Error>
        {
            Error.BadRequest("test.bad1", "Bad 1"),
            Error.BadRequest("test.bad2", "Bad 2")
        };

        PagedResult<string, Error> result = errors;

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    #endregion

    #region Error Array Implicit Operator

    [Fact]
    public void ImplicitOperator_ErrorArray_ShouldCreateFailureResult()
    {
        var errors = new Error[]
        {
            Error.BadRequest("test.bad1", "Bad 1"),
            Error.BadRequest("test.bad2", "Bad 2")
        };

        PagedResult<string, Error> result = errors;

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    #endregion
}
