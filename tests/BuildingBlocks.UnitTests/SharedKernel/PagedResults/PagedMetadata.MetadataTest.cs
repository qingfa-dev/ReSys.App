using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedMetadataMetadataTest
{
    [Fact]
    public void PagedMetadata_ShouldExposeSharedMetadata()
    {
        var metadata = PagedMetadata.From(2, 10, 25).WithKey("Filter", "active");

        var sharedMetadata = (IMetadata)metadata;

        sharedMetadata.Metadata["pagenumber"].Should().Be(2);
        sharedMetadata.Metadata["filter"].Should().Be("active");
    }
}