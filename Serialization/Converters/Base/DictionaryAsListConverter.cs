using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.Base
{
    public abstract class DictionaryAsListConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializer serializer)
        {
            // var sorted = value.Values.OrderBy(v => GetKey(v)).ToList();
            serializer.Serialize(writer, value.Values);
        }
        public override Dictionary<TKey, TValue> ReadJson(JsonReader reader, Type objectType,
            Dictionary<TKey, TValue> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException("Expected array to deserialize dictionary.");
            
            var values = serializer.Deserialize<List<TValue>>(reader);
            var dict = new Dictionary<TKey, TValue>(values.Count);
            
            foreach (var value in values)
            {
                var key = GetKey(value);
                if (key == null)
                    throw new JsonSerializationException($"Key returned null for an item");
                if (!dict.TryAdd(key, value))
                    throw new JsonSerializationException($"Duplicate key '{key}' found when building dictionary");
            }

            return dict;
        }

        protected abstract TKey GetKey(TValue value);
    }
}