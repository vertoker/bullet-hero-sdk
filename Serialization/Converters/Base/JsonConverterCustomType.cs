using System;
using BH.SDK.Models;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.Base
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
            
            writer.WriteStartArray();
            
            // writer.WritePropertyName(Names.TypeShort);
            serializer.Serialize(writer, GetCustomType(value));
            
            // writer.WritePropertyName(Names.ValueShort);
            // serialize via another serializer, it must NOT include this instance of converter
            SerializerDefault.Serialize(writer, value);
            
            writer.WriteEndArray();
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return default;
            if (reader.TokenType != JsonToken.StartArray) 
                throw new JsonSerializationException("Expected StartArray");

            reader.Read(); // to first array element, it's type (as "t")
            var customType = serializer.Deserialize<TType>(reader);

            reader.Read(); // to second array element, it's value (as "v")
            var targetType = GetType(customType);
            var value = (T)SerializerDefault.Deserialize(reader, targetType);

            reader.Read(); // to EndArray
            if (reader.TokenType != JsonToken.EndArray)
                throw new JsonSerializationException("Expected EndArray");

            return value;
        }

        public abstract TType GetCustomType(T value);
        public abstract Type GetType(TType customType);
    }
}