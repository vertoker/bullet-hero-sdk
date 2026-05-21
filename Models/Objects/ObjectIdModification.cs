using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class ObjectIdModification : ICopyable<ObjectIdModification>, IEquatable<ObjectIdModification>
    {
        [JsonProperty(Names.PrevObjectId)]
        public int PrevObjectId { get; set; }
        
        [JsonProperty(Names.NextObjectId)]
        public int NextObjectId { get; set; }

        public ObjectIdModification()
        {
            PrevObjectId = ObjectRules.UndefinedId;
            NextObjectId = ObjectRules.UndefinedId;
        }
        public ObjectIdModification(int prevObjectId, int nextObjectId)
        {
            PrevObjectId = prevObjectId;
            NextObjectId = nextObjectId;
        }

        public object Clone() => Copy();
        public ObjectIdModification Copy() => new(PrevObjectId, NextObjectId);

        public override bool Equals(object obj) => obj is ObjectIdModification value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(PrevObjectId, NextObjectId);

        public bool Equals(ObjectIdModification other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = PrevObjectId.Equals(other.PrevObjectId)
                         && NextObjectId.Equals(other.NextObjectId);
            return result;
        }
    }
}