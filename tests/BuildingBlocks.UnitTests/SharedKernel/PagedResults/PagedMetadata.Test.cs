namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedMetadataTest
{
    [Theory]
    [InlineData(1, 10, 100, 10, true, false, 10)]
    [InlineData(5, 20, 100, 5, true, true, 20)]
    [InlineData(10, 10, 100, 10, false, true, 10)]
    [InlineData(1, 1, 1, 1, false, false, 1)]
    public void Constructor_ShouldSetAllProperties(
        int pageNumber, int pageSize, long totalCount,
        int totalPages, bool hasNextPage, bool hasPreviousPage, int expectedCount)
    {
        var metadata = new PagedMetadata(pageNumber, pageSize, totalCount, totalPages, hasNextPage, hasPreviousPage, expectedCount);

        metadata.PageNumber.Should().Be(pageNumber);
        metadata.PageSize.Should().Be(pageSize);
        metadata.TotalCount.Should().Be(totalCount);
        metadata.TotalPages.Should().Be(totalPages);
        metadata.HasNextPage.Should().Be(hasNextPage);
        metadata.HasPreviousPage.Should().Be(hasPreviousPage);
        metadata.ItemCount.Should().Be(expectedCount);
    }
}
