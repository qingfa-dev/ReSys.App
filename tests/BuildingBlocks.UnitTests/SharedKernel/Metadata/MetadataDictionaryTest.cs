using BuildingBlocks.SharedKernel.Metadata;

namespace BuildingBlocks.UnitTests.SharedKernel.Metadata;

public class MetadataDictionaryTest
{
    [Fact]
    public void Create_ShouldUseCaseInsensitiveKeys()
    {
        var metadata = MetadataDictionary.Create();

        metadata["CorrelationId"] = "request-1";

        metadata["correlationid"].Should().Be("request-1");
    }

    [Fact]
    public void MetadataExtensions_ShouldSetGetAndMergeValues()
    {
        var source = new MetadataHolder().SetMetadata("TenantId", "tenant-1");
        var destination = new MetadataHolder().SetMetadata("Existing", true);

        destination.MergeMetadata(source);

        destination.GetMetadata("tenantid").Should().Be("tenant-1");
        destination.GetMetadata("existing").Should().Be(true);
    }

    private sealed class MetadataHolder : IMetadata
    {
        public IMetadataDictionary Metadata { get; } = MetadataDictionary.Create();
    }
}