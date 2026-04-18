using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Resources;

namespace BHSDK.Models.Resources
{
    public class FontResourceKey : ResourceKey
    {
        public override ResourceType Type => ResourceType.Font;
        
        public FontResourceKey()
        {
            
        }
        public FontResourceKey(ResourceUriType uriType, string uri) : base(uriType, uri)
        {
            
        }
    }
}