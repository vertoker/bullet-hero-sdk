using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class LevelMetaV1 : ILevelMeta
    {
        public string LevelName { get; set; }
        public Version LevelVersion { get; set; }
        public List<IAuthor> Authors { get; set; }

        public LevelMetaV1()
        {
            LevelName = string.Empty;
            LevelVersion = new Version();
            Authors = new List<IAuthor>();
        }
        public LevelMetaV1(string levelName, Version levelVersion, List<IAuthor> authors)
        {
            LevelName = levelName;
            LevelVersion = levelVersion;
            Authors = authors;
        }
    }
}