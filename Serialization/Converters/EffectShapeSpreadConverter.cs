using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class EffectShapeSpreadConverter : JsonConverter<IEffectShapeSpread>
    {
        public override void WriteJson(JsonWriter writer, IEffectShapeSpread value, JsonSerializer serializer)
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

        public override IEffectShapeSpread ReadJson(JsonReader reader, Type objectType, 
            IEffectShapeSpread existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) 
                throw new JsonSerializationException("Expected StartObject");

            EffectShapeSpreadType effectShapeSpreadType = default;
            IEffectShapeSpread value = null;

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
                        effectShapeSpreadType = serializer.Deserialize<EffectShapeSpreadType>(reader);
                        break;
                    case ConverterStatics.ValuePropertyName:
                        var type = ConverterStatics.GetEffectShapeSpreadType(effectShapeSpreadType);
                        value = (IEffectShapeSpread)serializer.Deserialize(reader, type);
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