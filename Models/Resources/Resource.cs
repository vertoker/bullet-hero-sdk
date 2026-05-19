using System;
using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public abstract class Resource : IEquatable<Resource>
    {
        public const int UninitializedId = 0;
        public const int MaxSourcesCount = 4;
        
        // Introduction into resourceId
        // By itself resourceId exists ONLY internally in runtime
        // In Level, for each resource type exists unique resourceId: audioResourceId, textureResourceId...
        // They work the same way and used by level objects (render texture, play audio etc...)
        
        // typedResourceId divided to 2 types: game-defined and user-defined
        // all game-defined starts with 0 and counts to positive (0, 1, 2, 3...)
        // all user-defined starts with -1 and counts to negative (-1, -2, -3, -4...)
        // game-defined is resources stored in game, each resource has permanent id
        // user-defined is resources stored externally: level folder, url etc..., they ids stores in Level itself
        
        // Internally, they converted from resourceId to resourceIndex with simple function
        // "resourceIndex = resourceId >= 0 ? resourceId : <count of game-defined resources> - resourceId - 1"
        // (in real code don't used resourceId, it uses typed id like textureResourceId and textureResourceIndex)
        // Count of game-defined resources (textures etc.) can be changed with Level version, this validates by validators
        
        [RuleNotNull, RuleCollectionMaxCount(MaxSourcesCount)]
        [JsonProperty(Names.Src)]
        public List<ResourceKey> Sources { get; set; }
        
        public abstract ResourceType Type { get; }

        protected Resource()
        {
            Sources = new List<ResourceKey>();
        }
        protected Resource(List<ResourceKey> sources)
        {
            Sources = sources;
        }

        public bool Equals(Resource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Sources.ListEquals(other.Sources)
                         && Type == other.Type;
            return result;
        }

        public override bool Equals(object obj) => obj is Resource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Sources.GetListHashCode(), (int)Type);
    }
}