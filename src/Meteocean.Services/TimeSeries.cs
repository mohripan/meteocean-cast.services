using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meteocean.Services
{
    /// <summary>A single (timestamp, value) sample. Nullable value allows gaps.</summary>
    [JsonConverter(typeof(SampleArrayJsonConverter))]
    public readonly record struct Sample(DateTime TimestampUtc, double? Value);

    /// <summary>Normalized, provider-agnostic time series in canonical units.</summary>
    public sealed record TimeSeriesDto(
        Guid SiteId,
        string Variable,
        string Unit,
        IReadOnlyList<Sample> Samples
    );

    /// <summary>
    /// JSON converter that serializes Samples as [timestampIso, value] arrays for compact payloads.
    /// </summary>
    public sealed class SampleArrayJsonConverter : JsonConverter<Sample>
    {
        public override Sample Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected array for Sample.");
            reader.Read();
            var ts = reader.GetDateTime();
            reader.Read();
            double? val = reader.TokenType == JsonTokenType.Null ? null : reader.GetDouble();
            reader.Read(); // EndArray
            return new Sample(DateTime.SpecifyKind(ts, DateTimeKind.Utc), val);
        }

        public override void Write(Utf8JsonWriter writer, Sample value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(value.TimestampUtc);
            if (value.Value is null) writer.WriteNullValue();
            else writer.WriteNumberValue(value.Value.Value);
            writer.WriteEndArray();
        }
    }

    /// <summary>Helper to register default JSON options for this library.</summary>
    public static class MeteoceanJson
    {
        public static JsonSerializerOptions CreateDefaultOptions()
        {
            var o = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = false
            };
            o.Converters.Add(new SampleArrayJsonConverter());
            return o;
        }
    }
}
