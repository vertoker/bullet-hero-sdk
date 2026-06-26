using System;
using BH.SDK.Models;
using BH.SDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.Base
{
    public abstract class JsonConverterData<T> : JsonConverter<SaveData<T>> where T : IData
    {
        public override void WriteJson(JsonWriter writer, SaveData<T> value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteStartObject();
            
            writer.WritePropertyName(Names.Version);
            serializer.Serialize(writer, value.Version);
            
            writer.WritePropertyName(Names.Value);
            var valueSerializer = OverrideValueSerializer ?? serializer;
            valueSerializer.Serialize(writer, value.GetData());
            
            writer.WriteEndObject();
        }
        
        public override SaveData<T> ReadJson(JsonReader reader, Type objectType, SaveData<T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException("Expected start of LevelData");

            var result = new SaveData<T>();

            while (reader.Read()) // to property name
            {
                if (reader.TokenType == JsonToken.EndObject) break;
                if (reader.TokenType != JsonToken.PropertyName)
                    throw new JsonSerializationException($"Expected property name, got {reader.TokenType}");

                ReadToken(reader, serializer, result);
            }
            
            return result;
        }

        private void ReadToken(JsonReader reader, JsonSerializer serializer, SaveData<T> result)
        {
            var propertyName = reader.Value?.ToString();
            reader.Read(); // to property value

            if (propertyName == Names.Version)
            {
                result.Version = serializer.Deserialize<Version>(reader);
            }
            else if (propertyName == Names.Value)
            {
                if (result.Version == null)
                    throw new JsonSerializationException("Missing version property");
                
                var levelType = GetType(result.Version);
                var valueSerializer = OverrideValueSerializer ?? serializer;
                result.SetData(valueSerializer.Deserialize(reader, levelType));
            }
            else
            {
                reader.Skip();
            }
        }

        protected abstract Type GetType(Version version);
        protected virtual JsonSerializer OverrideValueSerializer => null;
    }
}