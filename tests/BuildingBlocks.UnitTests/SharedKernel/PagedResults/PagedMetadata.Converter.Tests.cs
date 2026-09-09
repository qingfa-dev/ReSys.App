using System.Text.Json;

namespace BuildingBlocks.UnitTests.SharedKernel.PagedResults;

public class PagedMetadataConverterTests
{
    private readonly JsonSerializerOptions _options;

    public PagedMetadataConverterTests()
    {
        _options = new JsonSerializerOptions
        {
            Converters =
            {
                new PagedMetadataConverter()
            }
        };
    }

    [Fact]
    public void Deserialize_ShouldCreatePagedMetadata_FromPaginationFields()
    {
        const string json =
            """
            {
                "pageNumber": 2,
                "pageSize": 25,
                "totalCount": 100,
                "totalPages": 4,
                "hasNextPage": true,
                "hasPreviousPage": true,
                "itemCount": 25
            }
            """;

        var result = JsonSerializer.Deserialize<PagedMetadata>(
            json,
            _options);

        result.Should().NotBeNull();

        IReadOnlyDictionary<string, object?> dict = result;

        dict.Should().ContainKey("pageNumber");
        dict["pageNumber"].Should().Be(2);
        dict.Should().ContainKey("pageSize");
        dict["pageSize"].Should().Be(25);
        dict.Should().ContainKey("totalCount");
        dict["totalCount"].Should().Be(100L);
    }

    [Fact]
    public void Deserialize_ShouldUseDefaults_WhenPaginationFieldsAreMissing()
    {
        const string json = "{}";

        var result = JsonSerializer.Deserialize<PagedMetadata>(
            json,
            _options);

        result.Should().NotBeNull();

        IReadOnlyDictionary<string, object?> dict = result;

        dict.Should().ContainKey("pageNumber");
        dict["pageNumber"].Should().Be(PagedResultConstant.Defaults.PageNumber);
        dict.Should().ContainKey("pageSize");
        dict["pageSize"].Should().Be(PagedResultConstant.Defaults.PageSize);
        dict.Should().ContainKey("totalCount");
        dict["totalCount"].Should().Be(PagedResultConstant.Defaults.TotalCount);
    }

    [Fact]
    public void Deserialize_ShouldPreserveCustomMetadataValues()
    {
        const string json =
            """
            {
                "pageNumber": 1,
                "pageSize": 10,
                "totalCount": 50,
                "correlationId": "abc-123",
                "isCached": true,
                "priority": 5
            }
            """;

        var result = JsonSerializer.Deserialize<PagedMetadata>(
            json,
            _options);

        result.Should().NotBeNull();

        IReadOnlyDictionary<string, object?> dict = result;

        dict.Should().ContainKey("correlationId");
        dict["correlationId"].Should().Be("abc-123");
        dict.Should().ContainKey("isCached");
        dict["isCached"].Should().Be(true);
        dict.Should().ContainKey("priority");
        dict["priority"].Should().Be(5L);
    }

    [Fact]
    public void Serialize_ShouldWriteFlatJsonObject()
    {
        var metadata = PagedMetadata
            .From(
                pageNumber: 2,
                pageSize: 20,
                totalCount: 200)
            .WithKey("traceId", "xyz");

        var json = JsonSerializer.Serialize(
            metadata,
            _options);

        using var document = JsonDocument.Parse(json);

        var root = document.RootElement;

        root.GetProperty("traceId")
            .GetString()
            .Should()
            .Be("xyz");
    }

    [Fact]
    public void SerializeAndDeserialize_ShouldPreserveMetadata()
    {
        var original = PagedMetadata
            .From(
                pageNumber: 3,
                pageSize: 50,
                totalCount: 150)
            .WithKey("source", "api")
            .WithKey("enabled", true);

        var json = JsonSerializer.Serialize(
            original,
            _options);

        var result = JsonSerializer.Deserialize<PagedMetadata>(
            json,
            _options);

        result.Should().NotBeNull();

        IReadOnlyDictionary<string, object?> dict = result;

        dict.Should().ContainKey("source");
        dict["source"].Should().Be("api");
        dict.Should().ContainKey("enabled");
        dict["enabled"].Should().Be(true);
    }

    [Fact]
    public void Deserialize_ShouldIgnoreKnownComputedFields()
    {
        const string json =
            """
            {
                "pageNumber": 1,
                "pageSize": 10,
                "totalCount": 100,
                "totalPages": 10,
                "hasNextPage": true,
                "hasPreviousPage": false,
                "itemCount": 10
            }
            """;

        var result = JsonSerializer.Deserialize<PagedMetadata>(
            json,
            _options);

        result.Should().NotBeNull();

        IReadOnlyDictionary<string, object?> dict = result;

        dict.Should().NotContainKey("totalPages");
        dict.Should().NotContainKey("hasNextPage");
        dict.Should().NotContainKey("hasPreviousPage");
        dict.Should().NotContainKey("itemCount");
    }
}
