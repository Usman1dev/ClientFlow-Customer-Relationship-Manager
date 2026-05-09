using RUDCRM.Shared.DTOs;
using System.Text.Json;

namespace RUDCRM.Server.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<WeatherDto?> GetWeatherAsync(string city)
    {
        try
        {
            var apiKey = _configuration["WeatherApi:ApiKey"];
            var baseUrl = _configuration["WeatherApi:BaseUrl"];

            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_API_KEY_HERE")
            {
                _logger.LogWarning("OpenWeatherMap API key is not configured.");
                return null;
            }

            var url = $"{baseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Weather API returned status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new WeatherDto
            {
                City = root.GetProperty("name").GetString() ?? city,
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                FeelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                Description = root.GetProperty("weather")[0].GetProperty("description").GetString() ?? "N/A",
                Icon = root.GetProperty("weather")[0].GetProperty("icon").GetString() ?? "01d"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data for city: {City}", city);
            return null;
        }
    }
}
