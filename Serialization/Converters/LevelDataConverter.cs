using System;
using BHSDK.Models;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class LevelDataConverter : JsonConverter<LevelData>
    {
        private readonly CompatibilityService _compatibilityService;

        public LevelDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        public override void WriteJson(JsonWriter writer, LevelData value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteStartObject();
            
            writer.WritePropertyName(ModelNames.Version);
            serializer.Serialize(writer, value.Version);
            
            writer.WritePropertyName(ModelNames.Level);
            serializer.Serialize(writer, value.Level);
            
            writer.WriteEndObject();
        }

        public override LevelData ReadJson(JsonReader reader, Type objectType, LevelData existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException("Expected start of LevelData");

            var result = new LevelData();

            while (reader.Read()) // to property name
            {
                if (reader.TokenType == JsonToken.EndObject) break;
                if (reader.TokenType != JsonToken.PropertyName)
                    throw new JsonSerializationException($"Expected property name, got {reader.TokenType}");

                var propertyName = reader.Value?.ToString();
                reader.Read(); // to property value

                switch (propertyName)
                {
                    case ModelNames.Version:
                        result.Version = serializer.Deserialize<Version>(reader);
                        break;
                    case ModelNames.Level:
                        var levelType = _compatibilityService.GetLevelType(result.Version);
                        result.Level = (ILevel)serializer.Deserialize(reader, levelType);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
            
            if (result.Version == null)
                throw new JsonSerializationException("Missing version property");
            if (result.Level == null)
                throw new JsonSerializationException("Missing level property");
            
            return result;
        }
    }
}