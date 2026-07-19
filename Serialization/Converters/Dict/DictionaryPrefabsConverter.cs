using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryPrefabsConverter : DictionaryAsListConverter<PrefabId, Prefab>
    {
        protected override PrefabId GetKey(Prefab value) => value.PrefabId;
    }
}