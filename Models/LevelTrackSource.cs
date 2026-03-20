using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class LevelTrackSource
    {
        [JsonProperty("l")]
        public string Link { get; set; }
        
        [JsonProperty("lt")]
        public AudioLinkType LinkType { get; set; }
        
        [JsonProperty("st")]
        public float StartTime { get; set; }
        
        [JsonProperty("et")]
        public float EndTime { get; set; }

        public LevelTrackSource()
        {
            Link = string.Empty;
            LinkType = AudioLinkType.Undefined;
            StartTime = 0f;
            EndTime = 0f;
        }
        public LevelTrackSource(string link, AudioLinkType linkType, float startTime, float endTime)
        {
            Link = link;
            LinkType = linkType;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}