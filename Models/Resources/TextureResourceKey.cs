using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Resources;

namespace BHSDK.Models.Resources
{
    public class TextureResourceKey : ResourceKey
    {
        public override ResourceType Type => ResourceType.Texture;
        
        public TextureResourceKey()
        {
            
        }
        public TextureResourceKey(ResourceUriType uriType, string uri) : base(uriType, uri)
        {
            
        }
    }
}