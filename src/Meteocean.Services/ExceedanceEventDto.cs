namespace Meteocean.Services
{
    public enum ExceedanceStatus { Entered, Ongoing, Cleared }

    /// <summary>Represents a threshold exceedance state change.</summary>
    public sealed record ExceedanceEventDto(
        Guid Id,
        Guid SiteId,
        DateTime WhenUtc,
        string Expr,
        ExceedanceStatus Status,
        IReadOnlyDictionary<string, double?> Context,
        string Severity
    );
}
