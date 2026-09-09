namespace BuildingBlocks.SharedKernel.Metadata;

/// <summary>
/// Default mutable metadata dictionary.
/// </summary>
public sealed class MetadataDictionary : Dictionary<string, object>, IMetadataDictionary
{
	public const string Timestamp = nameof(Timestamp);
	public const string CorrelationId = nameof(CorrelationId);
	public const string RequestId = nameof(RequestId);
	public const string CausationId = nameof(CausationId);
	public const string EventId = nameof(EventId);
	public const string AggregateVersion = nameof(AggregateVersion);

	public MetadataDictionary()
		: base(StringComparer.OrdinalIgnoreCase)
	{
	}

	public static MetadataDictionary Create()
	{
		return new MetadataDictionary();
	}
}