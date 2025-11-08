namespace Meteocean.Services.Test
{
    public class ConstantsAndTopicsTests
    {
        [Fact]
        public void Variable_Strings_AreExpected()
        {
            Variable.Hs.Should().Be("hs");
            Variable.Tp.Should().Be("tp");
            Variable.Dir.Should().Be("dir");
            Variable.WindSpd.Should().Be("wind_spd");
            Variable.Wl.Should().Be("wl");
        }

        [Fact]
        public void Units_Strings_AreExpected()
        {
            Units.Meters.Should().Be("m");
            Units.Seconds.Should().Be("s");
            Units.DegreesFrom.Should().Be("deg_from");
            Units.MetersPerSecond.Should().Be("m/s");
        }

        [Fact]
        public void Topics_Strings_AreExpected()
        {
            Topics.SiteCreated.Should().Be("site.created");
            Topics.ThresholdsChanged.Should().Be("thresholds.changed");
            Topics.ForecastFetchRequested.Should().Be("forecast.fetch.requested");
            Topics.ForecastFetchCompleted.Should().Be("forecast.fetch.completed");
            Topics.ExceedanceTriggered.Should().Be("exceedance.triggered");
            Topics.NotificationDeliver.Should().Be("notification.deliver");
        }

        [Fact]
        public void Channel_Enum_HasExpectedValues()
        {
            ((int)Channel.Webhook).Should().Be(0);
            ((int)Channel.Slack).Should().Be(1);
            ((int)Channel.Teams).Should().Be(2);
            ((int)Channel.Email).Should().Be(3);
        }
    }
}
