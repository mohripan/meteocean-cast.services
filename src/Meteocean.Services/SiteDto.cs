namespace Meteocean.Services
{
    /// <summary>Represents a monitoring/operational site (point) for alerts and forecasts.</summary>
    public sealed record SiteDto(
        Guid Id,
        string Name,
        double Lat,
        double Lon,
        string Timezone,
        IReadOnlyList<string> Variables,
        IReadOnlyList<string> Providers,
        DateTime CreatedAtUtc
    );
}
