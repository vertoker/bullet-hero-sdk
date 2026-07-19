using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryFontResourcesConverter : DictionaryAsListConverter<FontResourceId, FontResource>
    {
        protected override FontResourceId GetKey(FontResource value) => value.FontResourceId;
    }
}