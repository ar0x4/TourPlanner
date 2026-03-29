namespace TourPlanner.BL;

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TourPlanner.Models;

public class RouteService : IRouteService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public RouteService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenRouteService:ApiKey"] ?? "";
    }

    public async Task<RouteInfo?> GetRouteAsync(string from, string to, string transportType)
    {
        var fromTask = GeocodeAsync(from);
        var toTask = GeocodeAsync(to);
        await Task.WhenAll(fromTask, toTask);

        var fromCoords = fromTask.Result;
        var toCoords = toTask.Result;

        if (fromCoords is null || toCoords is null)
            return null;

        var profile = transportType.ToLower() switch
        {
            "bike" => "cycling-regular",
            "hike" => "foot-hiking",
            "running" => "foot-walking",
            _ => "driving-car"
        };

        var request = new HttpRequestMessage(HttpMethod.Post,
            $"https://api.openrouteservice.org/v2/directions/{profile}/geojson");
        request.Headers.TryAddWithoutValidation("Authorization", _apiKey);
        request.Content = JsonContent.Create(new
        {
            coordinates = new[] { fromCoords, toCoords }
        });

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var feature = json.GetProperty("features")[0];
        var properties = feature.GetProperty("properties");
        var summary = properties.GetProperty("summary");

        var distanceMeters = summary.GetProperty("distance").GetDouble();
        var durationSeconds = summary.GetProperty("duration").GetDouble();

        var coordinates = feature.GetProperty("geometry")
            .GetProperty("coordinates")
            .EnumerateArray()
            .Select(c => new[] { c[0].GetDouble(), c[1].GetDouble() })
            .ToArray();

        var time = TimeSpan.FromSeconds(durationSeconds);
        return new RouteInfo
        {
            Distance = Math.Round(distanceMeters / 1000.0, 2),
            EstimatedTime = $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}",
            Coordinates = coordinates
        };
    }

    private async Task<double[]?> GeocodeAsync(string location)
    {
        var url = $"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(location)}&size=1&boundary.country=AT";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var features = json.GetProperty("features");
        if (features.GetArrayLength() == 0)
            return null;

        var coords = features[0].GetProperty("geometry").GetProperty("coordinates");
        return [coords[0].GetDouble(), coords[1].GetDouble()];
    }
}
