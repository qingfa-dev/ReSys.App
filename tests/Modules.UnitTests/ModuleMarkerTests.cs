namespace Modules.UnitTests;

public class ModuleMarkerTests
{
    [Fact]
    public void ModuleMarker_ShouldExist()
    {
        typeof(IModuleMarker).Should().NotBeNull();
    }
}