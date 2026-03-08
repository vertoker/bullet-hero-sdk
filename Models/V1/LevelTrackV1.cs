using System.Collections.Generic;
using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class LevelTrackV1 : ILevelTrack
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<ILevelTrackSource> Sources { get; set; } = new();
    }
}