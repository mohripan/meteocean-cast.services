namespace Meteocean.Services.Test
{
    public class TimeSeriesDtoJsonTests
    {
        [Fact]
        public void Serialize_TimeSeriesDto_SamplesIsArrayOfArrays()
        {
            var siteId = Guid.NewGuid();
            var dto = new TimeSeriesDto(
                SiteId: siteId,
                Variable: Variable.Hs,
                Unit: Units.Meters,
                Samples: new List<Sample>
                {
                new(new DateTime(2025,1,2,3,4,5, DateTimeKind.Utc), 1.1),
                new(new DateTime(2025,1,2,4,4,5, DateTimeKind.Utc), null)
                }
            );

            var options = MeteoceanJson.CreateDefaultOptions();
            var json = JsonSerializer.Serialize(dto, options);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            root.TryGetProperty("samples", out var samples).Should().BeTrue();
            samples.ValueKind.Should().Be(JsonValueKind.Array);
            samples[0].ValueKind.Should().Be(JsonValueKind.Array);
            samples[0].GetArrayLength().Should().Be(2);
            samples[1][1].ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public void Deserialize_TimeSeriesDto_Roundtrip()
        {
            var siteId = Guid.NewGuid();
            var dto = new TimeSeriesDto(
                SiteId: siteId,
                Variable: Variable.WindSpd,
                Unit: Units.MetersPerSecond,
                Samples: new List<Sample>
                {
                new(new DateTime(2025,1,2,3,4,5, DateTimeKind.Utc), 8.5),
                new(new DateTime(2025,1,2,4,4,5, DateTimeKind.Utc), 7.2),
                }
            );

            var json = JsonSerializer.Serialize(dto);
            var back = JsonSerializer.Deserialize<TimeSeriesDto>(json)!;

            back.SiteId.Should().Be(siteId);
            back.Variable.Should().Be(Variable.WindSpd);
            back.Unit.Should().Be(Units.MetersPerSecond);
            back.Samples.Should().HaveCount(2);
            back.Samples[0].Value.Should().BeApproximately(8.5, 1e-12);
        }
    }
}
