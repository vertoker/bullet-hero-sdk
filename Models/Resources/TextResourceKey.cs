using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Resources;

namespace BHSDK.Models.Resources
{
    public class TextResourceKey : ResourceKey
    {
        public override ResourceType Type => ResourceType.Text;
        
        public TextResourceKey()
        {
            
        }
        public TextResourceKey(ResourceUriType uriType, string uri) : base(uriType, uri)
        {
            
        }
    }
}