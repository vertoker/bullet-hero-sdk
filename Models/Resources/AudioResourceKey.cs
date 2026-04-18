using BHSDK.Models.Base;
using BHSDK.Models.Enum.Resources;

namespace BHSDK.Models.Resources
{
    public class AudioResourceKey : ResourceKey
    {
        public override ResourceType Type => ResourceType.Audio;

        public AudioResourceKey()
        {
            
        }
        public AudioResourceKey(ResourceUriType uriType, string uri) : base(uriType, uri)
        {
            
        }
    }
}