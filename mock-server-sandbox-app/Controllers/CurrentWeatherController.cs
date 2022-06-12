using Microsoft.AspNetCore.Mvc;

namespace mock_server_sandbox_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrentWeatherController : ControllerBase
    {
        private WeatherApiClient _client;

        private readonly Range[] _ranges;

        public CurrentWeatherController(WeatherApiClient client)
        {
            _client = client;
            _ranges = new[] {
                new Range(-459, 55,"Cold"),
                new Range(56, 65,"Cool"),
                new Range(66, 75,"Mild"),
                new Range(76, 85,"Warm"),
                new Range(85, 459,"Hot")
            };
        }

        [HttpGet()]
        public async Task<string> Get(string city, string provider="api")
        {
            var temperature = await _client.GetTemperature(city, provider);

            if(temperature == null)
            {
                return "None";
            }

            foreach (var range in _ranges)
            {
                if(range.Min <= temperature && temperature <= range.Max)
                {
                    return $"{range.Name}({temperature}°F)";
                }
            }

            return "None";
        }
    }

    public record Range (int Min, int Max, string Name);
}