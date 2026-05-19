using System;
using System.Collections.Generic;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Resources
{
    [RuleContainer]
    public class AudioResource : Resource, ICopyable<AudioResource>, IEquatable<AudioResource>
    {
        // id for user-defined resources, allowed only negative (started with -1, 0 is uninitialized)
        // more about resourceId and how it works, read in Resource.cs file
        
        [RuleMax(0)]
        [JsonProperty(Names.AudioResourceId)]
        public int AudioResourceId { get; set; }
        
        public override ResourceType Type => ResourceType.Audio;

        public AudioResource()
        {
            AudioResourceId = UninitializedId;
        }
        public AudioResource(int audioResourceId, List<ResourceKey> sources) : base(sources)
        {
            AudioResourceId = audioResourceId;
        }

        public object Clone() => Copy();
        public AudioResource Copy() => new(AudioResourceId, Sources.CopyList());

        public override bool Equals(object obj) => obj is AudioResource value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AudioResourceId);

        public bool Equals(AudioResource other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && AudioResourceId.Equals(other.AudioResourceId);
            return result;
        }
    }
}