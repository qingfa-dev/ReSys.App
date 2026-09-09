using System.Collections;

namespace BuildingBlocks.SharedKernel.PagedResults;

public sealed partial class PagedMetadata : IPagedMetadata, IReadOnlyDictionary<string, object?>
{
    object? IReadOnlyDictionary<string, object?>.this[string key] => _keyValues.TryGetValue(key, out var value) ? value : null;

    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => _keyValues.Keys;

    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => _keyValues.Values;

    int IReadOnlyCollection<KeyValuePair<string, object?>>.Count => _keyValues.Count;

    public bool ContainsKey(string key) => _keyValues.ContainsKey(key);

    public bool TryGetValue(string key, out object? value) => _keyValues.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _keyValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
