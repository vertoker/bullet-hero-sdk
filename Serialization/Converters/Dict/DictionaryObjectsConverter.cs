using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryObjectsConverter : DictionaryAsListConverter<ObjectId, RectObject>
    {
        protected override ObjectId GetKey(RectObject value) => value.ObjectId;
    }
}