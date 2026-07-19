using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryTextureResourcesConverter : DictionaryAsListConverter<TextureResourceId, TextureResource>
    {
        protected override TextureResourceId GetKey(TextureResource value) => value.TextureResourceId;
    }
}