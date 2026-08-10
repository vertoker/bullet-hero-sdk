using BH.SDK.Models.Data;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryCachedFontTextsConverter : DictionaryAsListConverter<FontResourceId, CachedFontText>
    {
        protected override FontResourceId GetKey(CachedFontText value) => value.FontResourceId;
    }
}
