using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.V1;

namespace BHSDK.Models.Interfaces
{
    public interface ILevelTrackSource : IModelReflection<LevelTrackSourceV1, ILevelTrackSource>
    {
        public string Link { get; set; }
        public AudioLinkType LinkType { get; set; }
        
        public float StartTime { get; set; }
        public float EndTime { get; set; }
    }
}