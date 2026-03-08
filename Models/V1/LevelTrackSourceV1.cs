using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class LevelTrackSourceV1 : ILevelTrackSource
    {
        public string Link { get; set; }
        public AudioLinkType LinkType { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }

        public LevelTrackSourceV1()
        {
            Link = string.Empty;
            LinkType = AudioLinkType.Undefined;
            StartTime = 0f;
            EndTime = 0f;
        }
        public LevelTrackSourceV1(string link, AudioLinkType linkType, float startTime, float endTime)
        {
            Link = link;
            LinkType = linkType;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}