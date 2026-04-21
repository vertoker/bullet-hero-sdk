using System.Collections.Generic;
using BHSDK.Models.Resources;
using Newtonsoft.Json;

namespace BHSDK.Models.Audio
{
    public class LevelTrack
    {
        public const int MaxSourcesCount = 4;
        
        // Same logic as Object.ObjectId, but only for audio and much simpler
        // 0 - undefined
        // 1, 2, 3... - user-defined audio
        // negative space is banned for consistency
        
        [JsonProperty(ModelNames.Id)]
        public int AudioId { get; set; }
        
        public const int UndefinedId = 0;
        
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
        
        [JsonProperty(ModelNames.Offset + ModelNames.Local + ModelNames.Time)]
        public float OffsetLocalTime { get; set; }
        
        // positive with 0 - game-defined (0 is silence), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(ModelNames.Audio + ModelNames.Resource + ModelNames.Id)]
        public int AudioResourceId { get; set; }
        
        [JsonProperty(ModelNames.AudioEffect)]
        public LevelTrackEffects Effects { get; set; }
        
        public LevelTrack()
        {
            AudioId = 0;
            Title = string.Empty;
            Author = string.Empty;
            StartFrame = 0;
            EndFrame = 0;
            OffsetLocalTime = 0f;
            AudioResourceId = 0;
            Effects = new LevelTrackEffects();
        }
        public LevelTrack(int audioId, string title, string author, int startFrame, int endFrame, float offsetLocalTime, 
            int audioResourceId, LevelTrackEffects effects)
        {
            AudioId = audioId;
            Title = title;
            Author = author;
            StartFrame = startFrame;
            EndFrame = endFrame;
            OffsetLocalTime = offsetLocalTime;
            AudioResourceId = audioResourceId;
            Effects = effects;
        }
    }
}