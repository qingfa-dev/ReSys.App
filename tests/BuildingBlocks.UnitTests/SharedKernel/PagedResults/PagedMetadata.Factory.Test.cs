namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedMetadataFactoryTest
{
    [Theory]
    [InlineData(1, 10, 100, 10, true, false, 10)]
    [InlineData(1, 10, 95, 10, true, false, 10)]
    [InlineData(10, 10, 95, 10, false, true, 5)]
    [InlineData(1, 10, 0, 0, false, false, 0)]
    [InlineData(2, 25, 50, 2, false, true, 25)]
    public void From_ShouldComputeDerivedFields(
        int pageNumber, int pageSize, long totalCount,
        int expectedTotalPages, bool expectedHasNextPage, bool expectedHasPreviousPage, int expectedCount)
    {
        var metadata = PagedMetadata.From(pageNumber, pageSize, totalCount);

        metadata.PageNumber.Should().Be(pageNumber);
        metadata.PageSize.Should().Be(pageSize);
        metadata.TotalCount.Should().Be(totalCount);
        metadata.TotalPages.Should().Be(expectedTotalPages);
        metadata.HasNextPage.Should().Be(expectedHasNextPage);
        metadata.HasPreviousPage.Should().Be(expectedHasPreviousPage);
        metadata.ItemCount.Should().Be(expectedCount);
    }

    [Fact]
    public void From_ZeroPageSize_ShouldReturnZeroCounts()
    {
        var metadata = PagedMetadata.From(1, 0, 100);

        metadata.TotalPages.Should().Be(0);
        metadata.ItemCount.Should().Be(0);
    }
}
