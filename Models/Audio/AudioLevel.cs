using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class AudioLevel
    {
        // because master channels created only in Unity
        public const int FrameTrackLimit = 16;
        
        [JsonProperty(Names.Tracks)]
        public List<LevelTrack> Tracks { get; set; }

        public AudioLevel()
        {
            Tracks = new List<LevelTrack>();
        }
        public AudioLevel(List<LevelTrack> tracks)
        {
            Tracks = tracks;
        }
    }
}