using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class ScreenLimitConverter : JsonConverter<IScreenLimit>
    {
        public override void WriteJson(JsonWriter writer, IScreenLimit value, JsonSerializer serializer)
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

        public override IScreenLimit ReadJson(JsonReader reader, Type objectType, IScreenLimit existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) 
                throw new JsonSerializationException("Expected StartObject");

            ScreenLimitType screenLimitType = default;
            IScreenLimit value = null;

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
                        screenLimitType = serializer.Deserialize<ScreenLimitType>(reader);
                        break;
                    case ConverterStatics.ValuePropertyName:
                        var type = ConverterStatics.GetScreenLimitType(screenLimitType);
                        value = (IScreenLimit)serializer.Deserialize(reader, type);
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