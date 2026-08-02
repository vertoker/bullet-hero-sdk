using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryModificationsConverter : DictionaryAsListConverter<ModificationKey, Modification>
    {
        protected override ModificationKey GetKey(Modification value) => value.Key;
    }
}
