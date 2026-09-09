namespace Api.IntegrationTests;

public class HealthEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetHealth_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/health", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAlive_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/alive", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
