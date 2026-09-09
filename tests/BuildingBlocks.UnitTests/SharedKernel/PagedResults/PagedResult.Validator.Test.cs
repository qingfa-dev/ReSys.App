namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedResultValidatorTest
{
    [Theory]
    [InlineData(1, 10, 100)]
    [InlineData(5, 25, 500)]
    [InlineData(1, 1, 0)]
    public void ValidatePaging_ValidParams_ShouldNotThrow(int pageNumber, int pageSize, long totalCount)
    {
        Action act = () => PagedResultValidator.ValidatePaging(pageNumber, pageSize, totalCount);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(0, 10, 100)]
    [InlineData(-1, 10, 100)]
    public void ValidatePaging_InvalidPageNumber_ShouldThrow(int pageNumber, int pageSize, long totalCount)
    {
        Action act = () => PagedResultValidator.ValidatePaging(pageNumber, pageSize, totalCount);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"{PagedResultConstant.Codes.InvalidPageNumber} : *");
    }

    [Theory]
    [InlineData(1, 0, 100)]
    [InlineData(1, -1, 100)]
    [InlineData(1, 1001, 100)]
    public void ValidatePaging_InvalidPageSize_ShouldThrow(int pageNumber, int pageSize, long totalCount)
    {
        Action act = () => PagedResultValidator.ValidatePaging(pageNumber, pageSize, totalCount);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"{PagedResultConstant.Codes.InvalidPageSize} : *");
    }

    [Theory]
    [InlineData(1, 10, -1)]
    public void ValidatePaging_InvalidTotalCount_ShouldThrow(int pageNumber, int pageSize, long totalCount)
    {
        Action act = () => PagedResultValidator.ValidatePaging(pageNumber, pageSize, totalCount);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"{PagedResultConstant.Codes.InvalidTotalCount} : *");
    }
}