namespace Meteocean.Services.Test
{
    public class FetchRequestAndInterfacesTests
    {
        [Fact]
        public void FetchRequest_StoresValues_AndEqualityWorks()
        {
            var siteId = Guid.NewGuid();
            var req = new FetchRequest(
                SiteId: siteId,
                Lat: -6.12,
                Lon: 106.75,
                Variables: new[] { Variable.Hs, Variable.WindSpd },
                FromUtc: new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ToUtc: new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            );

            req.SiteId.Should().Be(siteId);
            req.Variables.Should().Contain(Variable.WindSpd);

            var modified = req with { Lon = 107.00 };
            modified.Should().NotBe(req);
            modified.Lon.Should().Be(107.00);
        }

        private sealed class FakeProvider : IForecastProvider
        {
            public string Name => "fake";
            public Task<IReadOnlyList<TimeSeriesDto>> FetchAsync(FetchRequest request, CancellationToken ct = default)
            {
                IReadOnlyList<TimeSeriesDto> result = Array.Empty<TimeSeriesDto>();
                return Task.FromResult(result);
            }
        }

        [Fact]
        public async Task IForecastProvider_MinImplementation_Works()
        {
            var p = new FakeProvider();
            p.Name.Should().Be("fake");

            var req = new FetchRequest(Guid.NewGuid(), 0, 0, Array.Empty<string>(),
                DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

            var res = await p.FetchAsync(req);
            res.Should().NotBeNull();
            res.Should().BeEmpty();
        }
    }
}
