using BH.SDK.Models.Data;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryEffectsConverter : DictionaryAsListConverter<EffectId, EffectData>
    {
        protected override EffectId GetKey(EffectData value) => value.EffectId;
    }
}