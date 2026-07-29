using BH.SDK.Models.Data;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryThemesConverter : DictionaryAsListConverter<ThemeId, ThemeData>
    {
        protected override ThemeId GetKey(ThemeData value) => value.ThemeId;
    }
}