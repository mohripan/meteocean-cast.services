namespace Meteocean.Services
{
    /// <summary>Message topic names for brokered event flow.</summary>
    public static class Topics
    {
        public const string SiteCreated = "site.created";
        public const string ThresholdsChanged = "thresholds.changed";
        public const string ForecastFetchRequested = "forecast.fetch.requested";
        public const string ForecastFetchCompleted = "forecast.fetch.completed";
        public const string ExceedanceTriggered = "exceedance.triggered";
        public const string NotificationDeliver = "notification.deliver";
    }
}
