namespace Meteocean.Services
{
    public enum Channel { Webhook, Slack, Teams, Email }

    /// <summary>Alert subscription target.</summary>
    public sealed record SubscriptionDto(
        Guid Id,
        Guid SiteId,
        Channel Channel,
        string Target,
        string? Secret,
        bool Active,
        DateTime CreatedAtUtc
    );
}
