using System;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.Base
{
    public abstract class JsonConverterCustomType<T, TType> : JsonConverter<T>
    {
        public JsonSerializer SerializerDefault { get; }
        
        protected JsonConverterCustomType(JsonSerializer serializerDefault)
        {
            SerializerDefault = serializerDefault;
        }
        
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            writer.WriteStartObject();
            
            writer.WritePropertyName(SerializationUtils.TypePropertyName);
            serializer.Serialize(writer, GetCustomType(value));
            
            writer.WritePropertyName(SerializationUtils.ValuePropertyName);
            // serialize via another serializer, it must NOT include this instance of converter
            SerializerDefault.Serialize(writer, value);
            
            writer.WriteEndObject();
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return default;
            if (reader.TokenType != JsonToken.StartObject) 
                throw new JsonSerializationException("Expected StartObject");

            TType customType = default;
            T value = default;

            while (reader.Read()) // to property name
            {
                if (reader.TokenType == JsonToken.EndObject) break;
                if (reader.TokenType != JsonToken.PropertyName)
                    throw new JsonSerializationException($"Expected property name, got {reader.TokenType}");

                var propertyName = reader.Value?.ToString();
                reader.Read(); // to property value

                switch (propertyName)
                {
                    case SerializationUtils.TypePropertyName:
                        customType = serializer.Deserialize<TType>(reader);
                        break;
                    case SerializationUtils.ValuePropertyName:
                        var type = GetType(customType);
                        // deserialize via another serializer, it must NOT include this instance of converter
                        value = (T)SerializerDefault.Deserialize(reader, type);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return value ?? throw new JsonSerializationException("Missing value");
        }

        public abstract TType GetCustomType(T value);
        public abstract Type GetType(TType customType);
    }
}