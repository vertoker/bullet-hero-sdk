using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class EffectShapeConverter : JsonConverter<IEffectShape>
    {
        public override void WriteJson(JsonWriter writer, IEffectShape value, JsonSerializer serializer)
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

        public override IEffectShape ReadJson(JsonReader reader, Type objectType, 
            IEffectShape existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) 
                throw new JsonSerializationException("Expected StartObject");

            EffectShapeType effectShapeType = default;
            IEffectShape value = null;

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
                        effectShapeType = serializer.Deserialize<EffectShapeType>(reader);
                        break;
                    case ConverterStatics.ValuePropertyName:
                        var type = ConverterStatics.GetEffectShapeType(effectShapeType);
                        value = (IEffectShape)serializer.Deserialize(reader, type);
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