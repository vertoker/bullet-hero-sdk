using System;
using BH.SDK.Models.Interfaces.Primitives;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class PrimitiveIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IPrimitiveInt).IsAssignableFrom(objectType);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((((IPrimitiveInt)value)!).Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var value = Convert.ToInt32(reader.Value);
            return Activator.CreateInstance(objectType, value);
        }
    }
}
