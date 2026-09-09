namespace BuildingBlocks.UnitTests;

public class BuildingBlocksMarkerTests
{
    [Fact]
    public void BuildingBlocksMarker_ShouldExist()
    {
        typeof(IBuildingBlocksMarker).Should().NotBeNull();
    }
}
