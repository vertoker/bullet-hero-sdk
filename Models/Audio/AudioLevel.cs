using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class AudioLevel
    {
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