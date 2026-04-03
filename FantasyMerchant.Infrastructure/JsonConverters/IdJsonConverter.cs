using System.Text.Json;
using System.Text.Json.Serialization;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Infrastructure.JsonConverters;

public class IdJsonConverter : JsonConverter<Id>
{
    public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var guidString = reader.GetString();
        return new Id(Guid.Parse(guidString!));
    }

    public override void Write(Utf8JsonWriter writer, Id value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}
