using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.Base
{
    public abstract class JsonConverterData<T> : JsonConverter<T> where T : class, ISaveData, new()
    {
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteStartObject();
            
            writer.WritePropertyName(Names.Version);
            serializer.Serialize(writer, value.Version);
            
            writer.WritePropertyName(GetObjectPropertyName());
            var valueSerializer = OverrideValueSerializer ?? serializer;
            valueSerializer.Serialize(writer, value.GetData());
            
            writer.WriteEndObject();
        }
        
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException("Expected start of LevelData");

            var result = new T();
            var dataPropertyName = GetObjectPropertyName();

            while (reader.Read()) // to property name
            {
                if (reader.TokenType == JsonToken.EndObject) break;
                if (reader.TokenType != JsonToken.PropertyName)
                    throw new JsonSerializationException($"Expected property name, got {reader.TokenType}");

                var propertyName = reader.Value?.ToString();
                reader.Read(); // to property value

                if (propertyName == Names.Version)
                {
                    result.Version = serializer.Deserialize<Version>(reader);
                }
                else if (propertyName == dataPropertyName)
                {
                    var levelType = GetType(result.Version);
                    var valueSerializer = OverrideValueSerializer ?? serializer;
                    result.SetData(valueSerializer.Deserialize(reader, levelType));
                }
                else
                {
                    reader.Skip();
                }
            }
            
            if (result.Version == null)
                throw new JsonSerializationException("Missing version property");
            if (result.GetData() == null)
                throw new JsonSerializationException("Missing data property");
            
            return result;
        }

        protected abstract string GetObjectPropertyName();
        protected abstract Type GetType(Version version);
        protected virtual JsonSerializer OverrideValueSerializer => null;
    }
}