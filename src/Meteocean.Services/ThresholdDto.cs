namespace Meteocean.Services
{
    /// <summary>Time window for sustained/rolling conditions.</summary>
    public sealed record Window(int Minutes)
    {
        public TimeSpan ToTimeSpan() => TimeSpan.FromMinutes(Minutes);
    }

    /// <summary>Hysteresis parameters to reduce alert chatter.</summary>
    public sealed record Hysteresis(bool Enter, int ExitAfterMinutes);

    /// <summary>Threshold rule expressed in simple DSL (e.g., "hs &lt; 1.5 && wind_spd &lt; 10").</summary>
    public sealed record ThresholdDto(
        Guid Id,
        Guid SiteId,
        string Expr,
        Window Window,
        Hysteresis? Hysteresis,
        string Severity,
        DateTime CreatedAtUtc
    );
}
