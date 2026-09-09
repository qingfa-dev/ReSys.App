using System.Collections;

namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedMetadataInterfaceTest
{
    [Fact]
    public void Indexer_KnownKey_ShouldReturnValue()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        IReadOnlyDictionary<string, object?> dict = metadata;

        dict["pageNumber"].Should().Be(1);
        dict["pageSize"].Should().Be(10);
        dict["totalCount"].Should().Be(50L);
    }

    [Fact]
    public void Indexer_ComputedKey_ShouldReturnNull()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        IReadOnlyDictionary<string, object?> dict = metadata;

        dict["totalPages"].Should().BeNull();
        dict["hasNextPage"].Should().BeNull();
        dict["hasPreviousPage"].Should().BeNull();
        dict["itemCount"].Should().BeNull();
    }

    [Fact]
    public void Indexer_UnknownKey_ShouldReturnNull()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        ((IReadOnlyDictionary<string, object?>)metadata)["unknown"].Should().BeNull();
    }

    [Fact]
    public void ContainsKey_ExistingKey_ShouldReturnTrue()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        metadata.ContainsKey("pageNumber").Should().BeTrue();
    }

    [Fact]
    public void ContainsKey_NonExistingKey_ShouldReturnFalse()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        metadata.ContainsKey("unknown").Should().BeFalse();
    }

    [Fact]
    public void TryGetValue_ExistingKey_ShouldReturnTrueAndValue()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        var result = metadata.TryGetValue("pageNumber", out var value);

        result.Should().BeTrue();
        value.Should().Be(1);
    }

    [Fact]
    public void TryGetValue_NonExistingKey_ShouldReturnFalse()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        var result = metadata.TryGetValue("unknown", out _);

        result.Should().BeFalse();
    }

    [Fact]
    public void Keys_ShouldContainBaseKeys()
    {
        var metadata = PagedMetadata.From(1, 10, 50);
        IReadOnlyDictionary<string, object?> dict = metadata;

        dict.Keys.Should().Contain(new[]
        {
            PagedMetadata.KeyPageNumber,
            PagedMetadata.KeyPageSize,
            PagedMetadata.KeyTotalCount
        });
    }

    [Fact]
    public void Count_ShouldReturnNumberOfEntries()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        var dict = (IReadOnlyDictionary<string, object?>)metadata;

        dict.Count.Should().Be(3);
    }

    [Fact]
    public void GetEnumerator_ShouldEnumerateAllEntries()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        var entries = metadata.ToList();

        entries.Should().HaveCount(3);
        entries.Should().Contain(e => e.Key == "pageNumber" && e.Value!.Equals(1));
    }

    [Fact]
    public void Values_ShouldReturnAllValues()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        IEnumerable<object?> values = ((IReadOnlyDictionary<string, object?>)metadata).Values;

        values.Should().Contain(1);
        values.Should().Contain(50L);
    }

    [Fact]
    public void NonGenericGetEnumerator_ShouldWork()
    {
        var metadata = PagedMetadata.From(1, 10, 50);

        var entries = ((IEnumerable)metadata).GetEnumerator();

        entries.MoveNext().Should().BeTrue();
    }
}
