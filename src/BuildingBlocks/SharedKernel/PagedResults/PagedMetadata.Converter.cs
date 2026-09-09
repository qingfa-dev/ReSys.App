using System.Text.Json;
using System.Text.Json.Serialization;

namespace BuildingBlocks.SharedKernel.PagedResults;

/// <summary>
/// Custom JSON converter for <see cref="PagedMetadata"/>.
/// Serializes/deserializes as a flat dictionary while ensuring
/// deserialization always returns a fully constructed instance.
/// </summary>
public sealed class PagedMetadataConverter : JsonConverter<PagedMetadata>
{
    private static readonly HashSet<string> KnownKeys =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "pageNumber",
            "pageSize",
            "totalCount",
            "totalPages",
            "hasNextPage",
            "hasPreviousPage",
            "itemCount"
        };

    public override PagedMetadata Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var metadata = document.RootElement
            .EnumerateObject()
            .ToDictionary(
                p => p.Name,
                p => p.Value,
                StringComparer.OrdinalIgnoreCase);

        var pageNumber = GetInt32(
            metadata,
            "pageNumber",
            PagedResultConstant.Defaults.PageNumber);

        var pageSize = GetInt32(
            metadata,
            "pageSize",
            PagedResultConstant.Defaults.PageSize);

        var totalCount = GetInt64(
            metadata,
            "totalCount",
            PagedResultConstant.Defaults.TotalCount);

        var result = PagedMetadata.From(
            pageNumber,
            pageSize,
            totalCount);

        foreach (var (key, value) in metadata)
        {
            if (!KnownKeys.Contains(key))
            {
                result = result.WithKey(
                    key,
                    ConvertJsonValue(value));
            }
        }

        return result;
    }

    public override void Write(
        Utf8JsonWriter writer,
        PagedMetadata value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var (key, item) in value)
        {
            writer.WritePropertyName(key);
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndObject();
    }

    private static int GetInt32(
        IReadOnlyDictionary<string, JsonElement> dictionary,
        string key,
        int fallback)
    {
        return dictionary.TryGetValue(key, out var value) &&
               value.TryGetInt32(out var result)
            ? result
            : fallback;
    }

    private static long GetInt64(
        IReadOnlyDictionary<string, JsonElement> dictionary,
        string key,
        long fallback)
    {
        return dictionary.TryGetValue(key, out var value) &&
               value.TryGetInt64(out var result)
            ? result
            : fallback;
    }

    private static object? ConvertJsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt64(out var l) => l,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.Clone()
        };
    }
}