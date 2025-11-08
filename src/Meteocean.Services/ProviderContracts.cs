namespace Meteocean.Services
{
    /// <summary>Request to retrieve forecast/nowcast data for a site.</summary>
    public sealed record FetchRequest(
        Guid SiteId,
        double Lat,
        double Lon,
        IReadOnlyList<string> Variables,
        DateTime FromUtc,
        DateTime ToUtc
    );

    /// <summary>Abstraction for any forecast provider (Open-Meteo, ERDDAP WW3, MIKE, etc.).</summary>
    public interface IForecastProvider
    {
        string Name { get; } // e.g., "openmeteo"
        Task<IReadOnlyList<TimeSeriesDto>> FetchAsync(FetchRequest request, CancellationToken ct = default);
    }

    /// <summary>Evaluates threshold rules on normalized time series.</summary>
    public interface IRuleEvaluator
    {
        Task<IEnumerable<ExceedanceEventDto>> EvaluateAsync(Guid siteId, IEnumerable<TimeSeriesDto> series, CancellationToken ct = default);
    }
}
