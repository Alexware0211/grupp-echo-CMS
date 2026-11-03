namespace Kvarter.Web.Data.Entities;

public class WeatherForecast
{
    public int Id { get; set; }

    // "stockholm" for the city-wide widget, or a Place page's key for place-specific weather.
    public string LocationKey { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateOnly Date { get; set; }

    public double TemperatureMin { get; set; }

    public double TemperatureMax { get; set; }

    // WMO weather code from the API (0 = clear sky, 61 = rain, ...).
    public int WeatherCode { get; set; }

    public double? PrecipitationMm { get; set; }

    public double? WindSpeedMs { get; set; }

    public DateTime FetchedAtUtc { get; set; } = DateTime.UtcNow;
}
