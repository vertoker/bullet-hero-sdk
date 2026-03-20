using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class ColorConverter : JsonConverter<IColor>
    {
        public override void WriteJson(JsonWriter writer, IColor value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteStartObject();
            
            writer.WritePropertyName("t");
            serializer.Serialize(writer, value.Type);
            
            writer.WritePropertyName("v");
            serializer.Serialize(writer, value);
            
            writer.WriteEndObject();
        }

        public override IColor ReadJson(JsonReader reader, Type objectType, IColor existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) 
                throw new JsonSerializationException("Expected StartObject");

            ColorType colorType = default;
            IColor value = null;

            while (reader.Read()) // to property name
            {
                if (reader.TokenType == JsonToken.EndObject) break;
                if (reader.TokenType != JsonToken.PropertyName)
                    throw new JsonSerializationException($"Expected property name, got {reader.TokenType}");

                var propertyName = reader.Value?.ToString();
                reader.Read(); // to property value

                switch (propertyName)
                {
                    case ConverterStatics.TypePropertyName:
                        colorType = serializer.Deserialize<ColorType>(reader);
                        break;
                    case ConverterStatics.ValuePropertyName:
                        var type = ConverterStatics.GetColorType(colorType);
                        value = (IColor)serializer.Deserialize(reader, type);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return value ?? throw new JsonSerializationException("Missing value");
        }
    }
}