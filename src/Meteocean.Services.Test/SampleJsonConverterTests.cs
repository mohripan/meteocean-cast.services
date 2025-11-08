namespace Meteocean.Services.Test
{
    public class SampleJsonConverterTests
    {
        [Fact]
        public void Serialize_Sample_WithValue_WritesArrayShape()
        {
            var ts = new DateTime(2025, 01, 02, 03, 04, 05, DateTimeKind.Utc);
            var s = new Sample(ts, 1.23);

            var json = JsonSerializer.Serialize(s); // attribute-based converter is applied

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            root.ValueKind.Should().Be(JsonValueKind.Array);
            root.GetArrayLength().Should().Be(2);
            root[0].GetString().Should().Be("2025-01-02T03:04:05Z");
            root[1].GetDouble().Should().BeApproximately(1.23, 1e-12);
        }

        [Fact]
        public void Serialize_Sample_WithNullValue_EmitsNull()
        {
            var ts = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var s = new Sample(ts, null);

            var json = JsonSerializer.Serialize(s);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            root[1].ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public void Deserialize_Sample_Array_ReadsValue_AndUtcKind()
        {
            const string json = @"[ ""2025-01-02T03:04:05Z"", 2.5 ]";

            var s = JsonSerializer.Deserialize<Sample>(json);

            s.Should().NotBeNull();
            s!.Value.Should().Be(2.5);
            s.Value!.Value.Should().BeApproximately(2.5, 1e-12);
            s.TimestampUtc.Kind.Should().Be(DateTimeKind.Utc);
            s.TimestampUtc.Should().Be(new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc));
        }

        [Fact]
        public void Deserialize_Sample_WithNullValue_Works()
        {
            const string json = @"[ ""2025-11-08T00:00:00Z"", null ]";
            var s = JsonSerializer.Deserialize<Sample>(json);

            s.Should().NotBeNull();
            s!.Value.Should().BeNull();
        }

        [Fact]
        public void Deserialize_Sample_InvalidShape_ThrowsJsonException()
        {
            const string json = @"{ ""ts"": ""2025-01-01T00:00:00Z"", ""val"": 1.0 }";
            Action act = () => JsonSerializer.Deserialize<Sample>(json);
            act.Should().Throw<JsonException>(); // converter expects an array, not an object
        }

        [Fact]
        public void Deserialize_Sample_EmptyArray_Throws()
        {
            const string json = "[]"; // StartArray then EndArray; converter will fail on GetDateTime
            Action act = () => JsonSerializer.Deserialize<Sample>(json);
            act.Should().Throw<Exception>(); // Implementation throws (InvalidOperationException/JsonException), assert generically
        }

        [Fact(Skip = "Known gap: converter does not validate EndArray after value; extra elements may be consumed incorrectly.")]
        public void Deserialize_Sample_ExtraElements_ShouldFailOrBeRejected()
        {
            // The converter reads: StartArray -> timestamp -> value -> Read() expecting EndArray (no check)
            // With an extra element, the reader may not be at EndArray; robust converter should reject this.
            const string json = @"[ ""2025-01-02T03:04:05Z"", 1.0, ""extra"" ]";
            Action act = () => JsonSerializer.Deserialize<Sample>(json);
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Culture_Independence_Serialize_DoesNotUseCurrentCultureForNumbers()
        {
            var ts = new DateTime(2025, 01, 02, 03, 04, 05, DateTimeKind.Utc);
            var s = new Sample(ts, 1.23);

            var before = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
                var json = JsonSerializer.Serialize(s);
                json.Should().Contain("1.23"); // not "1,23"
            }
            finally
            {
                System.Globalization.CultureInfo.CurrentCulture = before;
            }
        }

        [Fact]
        public void Roundtrip_Sample_List_PreservesValues()
        {
            var list = new[] {
            new Sample(new DateTime(2025, 01, 02, 03, 04, 05, DateTimeKind.Utc), 1.23),
            new Sample(new DateTime(2025, 01, 02, 04, 04, 05, DateTimeKind.Utc), null),
        };

            var json = JsonSerializer.Serialize(list);
            var back = JsonSerializer.Deserialize<Sample[]>(json)!;

            back.Should().HaveCount(2);
            back[0].Value.Should().BeApproximately(1.23, 1e-12);
            back[1].Value.Should().BeNull();
        }
    }
}
