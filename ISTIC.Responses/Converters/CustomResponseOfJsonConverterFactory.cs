using System.Text.Json.Serialization;
using System.Text.Json;
using ISTIC.Responses.Core;

namespace ISTIC.Responses.Converters;

public class CustomResponseOfJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(CustomResponseOf<,>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var genericArguments = typeToConvert.GetGenericArguments();
        var converterType = typeof(CustomResponseOfJsonConverter<,>).MakeGenericType(genericArguments[0], genericArguments[1]);

        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}
