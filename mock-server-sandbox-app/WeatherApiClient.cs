using System.Text.Json;
using System.Text.Json.Serialization;

namespace mock_server_sandbox_app
{
public class WeatherApiClient
{
    private readonly IHttpClientFactory _factory;

    private readonly Settings _settings;

    public WeatherApiClient(IHttpClientFactory factory, Settings settings)
    {
        _factory = factory;
        _settings = settings;
    }

    public async Task<decimal?> GetTemperature(string city, string provider)
    {
        var client = _factory.CreateClient(provider);

        var httpResponse = await client.GetAsync($"v1/current.json?key={_settings.Key}&q={city}&aqi=no");

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        httpResponse.EnsureSuccessStatusCode();

        var responseBody = await httpResponse.Content.ReadAsStringAsync();

        var response = JsonSerializer.Deserialize<Response>(responseBody, options);

        return response?.Current?.TemperatureF;
    }

    public class Settings
    {
        public string? Uri { get; set; }
        public string? Key { get; set; }
        public string? MockUri { get; set; }
    }

    public class Response
    {
        public Current? Current { get; set; }
    }

    public class Current
    {
        [JsonPropertyName("temp_f")]
        public decimal TemperatureF { get; set; }
    }
}
}