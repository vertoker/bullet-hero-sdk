using BH.SDK.Models.Objects;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryObjectsConverter : DictionaryAsListConverter<int, Object>
    {
        protected override int GetKey(Object value) => value.ObjectId;
    }
}