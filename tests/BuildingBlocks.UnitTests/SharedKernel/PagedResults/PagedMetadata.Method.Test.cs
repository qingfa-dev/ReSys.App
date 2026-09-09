namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedMetadataMethodTest
{
    [Fact]
    public void WithKey_ShouldAddCustomKey()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        var updated = metadata.WithKey("sortBy", "name");

        IReadOnlyDictionary<string, object?> dict = updated;
        dict["sortBy"].Should().Be("name");
        updated.PageNumber.Should().Be(1);
    }

    [Fact]
    public void WithKey_ShouldNotMutateOriginal()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        _ = metadata.WithKey("sortBy", "name");

        metadata.ContainsKey("sortBy").Should().BeFalse();
    }

    [Fact]
    public void WithKey_PreservesExistingCustomKeys()
    {
        var metadata = PagedMetadata.From(1, 10, 50)
            .WithKey("sortBy", "name");

        var updated = metadata.WithKey("filter", "active");

        IReadOnlyDictionary<string, object?> dict = updated;
        dict["sortBy"].Should().Be("name");
        dict["filter"].Should().Be("active");
    }

    [Fact]
    public void WithPage_ShouldReturnNewMetadataWithUpdatedPage()
    {
        var metadata = PagedMetadata.From(1, 10, 100);

        var updated = metadata.WithPage(5);

        updated.PageNumber.Should().Be(5);
        updated.PageSize.Should().Be(10);
    }

    [Fact]
    public void WithPage_PreservesCustomKeys()
    {
        var metadata = PagedMetadata.From(1, 10, 100)
            .WithKey("sortBy", "name");

        var updated = metadata.WithPage(5);

        IReadOnlyDictionary<string, object?> dict = updated;
        dict["sortBy"].Should().Be("name");
    }

    [Fact]
    public void WithPageSize_ShouldReturnNewMetadataWithUpdatedPageSize()
    {
        var metadata = PagedMetadata.From(1, 10, 100);

        var updated = metadata.WithPageSize(25);

        updated.PageSize.Should().Be(25);
        updated.PageNumber.Should().Be(1);
    }

    [Fact]
    public void WithPageSize_PreservesCustomKeys()
    {
        var metadata = PagedMetadata.From(1, 10, 100)
            .WithKey("sortBy", "name");

        var updated = metadata.WithPageSize(25);

        IReadOnlyDictionary<string, object?> dict = updated;
        dict["sortBy"].Should().Be("name");
    }
}