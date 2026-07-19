using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryThemesConverter : DictionaryAsListConverter<ThemeId, Theme>
    {
        protected override ThemeId GetKey(Theme value) => value.ThemeId;
    }
}