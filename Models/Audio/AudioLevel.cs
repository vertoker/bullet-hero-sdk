using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class AudioLevel
    {
        // temporary solution because master channels created only in Unity
        public const int FrameTrackLimit = 16;
        
        [JsonProperty(ModelNames.Track)]
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