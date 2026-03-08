using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class LevelTrackSourceV1 : ILevelTrackSource
    {
        public string Link { get; set; } = string.Empty;
        public AudioLinkType LinkType { get; set; } = AudioLinkType.Undefined;
        public float StartTime { get; set; } = 0f;
        public float EndTime { get; set; } = 0f;
    }
}