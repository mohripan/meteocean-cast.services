namespace Meteocean.Services.Test
{
    public class DtoAndRecordsTests
    {
        [Fact]
        public void SiteDto_CreatesAndExposesProperties()
        {
            var id = Guid.NewGuid();
            var site = new SiteDto(
                Id: id,
                Name: "Jakarta Port",
                Lat: -6.12,
                Lon: 106.75,
                Timezone: "Asia/Jakarta",
                Variables: new[] { Variable.Hs, Variable.WindSpd },
                Providers: new[] { "openmeteo" },
                CreatedAtUtc: DateTime.UtcNow
            );

            site.Id.Should().Be(id);
            site.Name.Should().Be("Jakarta Port");
            site.Variables.Should().Contain(Variable.Hs);
            site.Providers.Should().ContainSingle().Which.Should().Be("openmeteo");
        }

        [Fact]
        public void ThresholdDto_CanBeConstructed()
        {
            var t = new ThresholdDto(
                Id: Guid.NewGuid(),
                SiteId: Guid.NewGuid(),
                Expr: "hs < 1.5 && wind_spd < 10",
                Window: new Window(120),
                Hysteresis: new Hysteresis(true, 60),
                Severity: "watch",
                CreatedAtUtc: DateTime.UtcNow
            );

            t.Expr.Should().Contain("hs < 1.5");
            t.Window.Minutes.Should().Be(120);
            t.Hysteresis!.ExitAfterMinutes.Should().Be(60);
        }

        [Fact]
        public void ExceedanceEventDto_Equality_And_WithExpression()
        {
            var baseId = Guid.NewGuid();
            var ctx = new Dictionary<string, double?> { ["hs"] = 1.2, ["wind_spd"] = 8.0 };

            var e1 = new ExceedanceEventDto(
                Id: baseId,
                SiteId: Guid.NewGuid(),
                WhenUtc: DateTime.UtcNow,
                Expr: "hs < 1.5",
                Status: ExceedanceStatus.Entered,
                Context: ctx,
                Severity: "watch"
            );

            var e2 = e1 with { Status = ExceedanceStatus.Ongoing };

            e1.Should().NotBe(e2);
            e1.Id.Should().Be(baseId);
            e2.Status.Should().Be(ExceedanceStatus.Ongoing);
            e2.Context.Should().ContainKey("hs");
        }

        [Fact]
        public void SubscriptionDto_CanBeConstructed()
        {
            var s = new SubscriptionDto(
                Id: Guid.NewGuid(),
                SiteId: Guid.NewGuid(),
                Channel: Channel.Webhook,
                Target: "https://example.com/hooks/abc",
                Secret: "hmac-secret",
                Active: true,
                CreatedAtUtc: DateTime.UtcNow
            );

            s.Channel.Should().Be(Channel.Webhook);
            s.Active.Should().BeTrue();
            s.Target.Should().StartWith("https://");
        }
    }
}
