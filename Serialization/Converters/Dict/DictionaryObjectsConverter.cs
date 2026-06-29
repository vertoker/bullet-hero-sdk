using BH.SDK.Models.Objects;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryObjectsConverter : DictionaryAsListConverter<int, RectObject>
    {
        protected override int GetKey(RectObject value) => value.ObjectId;
    }
}