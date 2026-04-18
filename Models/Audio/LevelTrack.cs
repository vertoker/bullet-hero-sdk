using System.Collections.Generic;
using BHSDK.Models.Resources;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class LevelTrack
    {
        public const int MaxSourcesCount = 4;
        
        [JsonProperty(ModelNames.Title)]
        public string Title { get; set; }
        
        [JsonProperty(ModelNames.Author)]
        public string Author { get; set; }
        
        // TODO add audio parsers for this services
        // "Spotify", "https://open.spotify.com/artist/{0}"
        // "SoundCloud", "https://soundcloud.com/{0}"
        // "Bandcamp", "https://{0}.bandcamp.com"
        // "Youtube Music", "https://music.youtube.com/channel/{0}"
        // "Newgrounds", "https://{0}.newgrounds.com/"
        
        [JsonProperty(ModelNames.Start + ModelNames.Frame)]
        public int StartFrame { get; set; }
        
        [JsonProperty(ModelNames.End + ModelNames.Frame)]
        public int EndFrame { get; set; }
        
        [JsonProperty(ModelNames.Source)]
        public List<AudioResourceKey> Sources { get; set; }
        
        [JsonProperty(ModelNames.AudioEffect)] [CanBeNull]
        public LevelTrackEffects Effects { get; set; }
        
        public bool HasEffects() => Effects != null;

        public LevelTrack()
        {
            Title = string.Empty;
            Author = string.Empty;
            StartFrame = 0;
            EndFrame = 0;
            Sources = new List<AudioResourceKey>();
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(string title, string author, int startFrame, int endFrame, 
            List<AudioResourceKey> sources, LevelTrackEffects effects)
        {
            Title = title;
            Author = author;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Sources = sources;
            Effects = effects;
        }
    }
}