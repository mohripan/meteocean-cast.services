namespace Meteocean.Services.Test
{
    public class MeteoceanJsonOptionsTests
    {
        [Fact]
        public void CreateDefaultOptions_Registers_SampleArrayJsonConverter()
        {
            var options = MeteoceanJson.CreateDefaultOptions();
            options.Should().NotBeNull();

            // Ensure converter type is included
            var found = options.Converters.Any(c => c is SampleArrayJsonConverter);
            found.Should().BeTrue();

            // Should produce array-shape for Sample
            var s = new Sample(new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc), 2.0);
            var json = System.Text.Json.JsonSerializer.Serialize(s, options);
            json.Should().StartWith("[");
            json.Should().Contain("2025-01-02T03:04:05Z");
        }
    }
}
