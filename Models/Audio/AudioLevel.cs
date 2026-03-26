using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class AudioLevel
    {
        public const int TrackLimit = 16; // temporary solution because master channels created only in Unity
        
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