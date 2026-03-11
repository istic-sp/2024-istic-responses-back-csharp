using System.Text.Json.Serialization;
using System.Text.Json;
using ISTIC.Responses.Core;

namespace ISTIC.Responses.Converters;

public class CustomResponseOfJsonConverter<TResult, TError> : JsonConverter<CustomResponseOf<TResult, TError>>
{
    public override CustomResponseOf<TResult, TError> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, CustomResponseOf<TResult, TError> value, JsonSerializerOptions options)
    {
        if (value.CustomError != null)
        {
            JsonSerializer.Serialize(writer, value.CustomError, options);
        }
        else
        {
            JsonSerializer.Serialize(writer, value.Result, options);
        }
    }
}
