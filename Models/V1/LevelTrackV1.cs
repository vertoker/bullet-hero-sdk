using System.Collections.Generic;
using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class LevelTrackV1 : ILevelTrack
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<ILevelTrackSource> Sources { get; set; }

        public LevelTrackV1()
        {
            Title = string.Empty;
            Author = string.Empty;
            Sources = new List<ILevelTrackSource>();
        }
        public LevelTrackV1(string title, string author, List<ILevelTrackSource> sources)
        {
            Title = title;
            Author = author;
            Sources = sources;
        }
    }
}