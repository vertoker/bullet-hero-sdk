using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class AudioResourceKey : ResourceKey
    {
        [JsonProperty(ModelNames.Start + ModelNames.Time)]
        public float StartTime { get; set; }

        public override ResourceType Type => ResourceType.Audio;

        public AudioResourceKey()
        {
            StartTime = 0f;
        }
        public AudioResourceKey(float startTime, ResourceUriType uriType, string uri) : base(uriType, uri)
        {
            StartTime = startTime;
        }
    }
}