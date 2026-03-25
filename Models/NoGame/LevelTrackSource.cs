using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    public class LevelTrackSource
    {
        [JsonProperty(ModelNames.Link)]
        public string Link { get; set; }
        
        [JsonProperty(ModelNames.Link + ModelNames.Type)]
        public AudioLinkType LinkType { get; set; }
        
        [JsonProperty(ModelNames.Start + ModelNames.Time)]
        public float StartTime { get; set; }
        
        [JsonProperty(ModelNames.End + ModelNames.Time)]
        public float EndTime { get; set; }
        
        // TODO add audio parsers for this services
        // "Spotify", "https://open.spotify.com/artist/{0}"
        // "SoundCloud", "https://soundcloud.com/{0}"
        // "Bandcamp", "https://{0}.bandcamp.com"
        // "Youtube Music", "https://music.youtube.com/channel/{0}"
        // "Newgrounds", "https://{0}.newgrounds.com/"

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