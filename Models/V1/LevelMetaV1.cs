using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class LevelMetaV1 : ILevelMeta
    {
        public string LevelName { get; set; } = string.Empty;
        public Version LevelVersion { get; set; } = new();
        public string MusicTitle { get; set; } = string.Empty;
        public string MusicAuthor { get; set; } = string.Empty;
        public List<IAuthor> Authors { get; set; } = new();
    }
}