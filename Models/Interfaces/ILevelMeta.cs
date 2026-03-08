using System;
using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.V1;

namespace BHSDK.Models.Interfaces
{
    public interface ILevelMeta : IModelReflection<LevelMetaV1, ILevelMeta>
    {
        public string LevelName { get; set; }
        public Version LevelVersion { get; set; }
        public List<IAuthor> Authors { get; set; }
    }
}