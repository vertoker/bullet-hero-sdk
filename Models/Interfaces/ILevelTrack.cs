using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.V1;

namespace BHSDK.Models.Interfaces
{
    public interface ILevelTrack : IModelReflection<LevelTrackV1, ILevelTrack>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<ILevelTrackSource> Sources { get; set; }
    }
}