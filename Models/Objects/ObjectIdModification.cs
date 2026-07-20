using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class ObjectIdModification : IModel<ObjectIdModification>
    {
        [RuleObjectIdValid]
        [JsonProperty(Names.PrevObjectId)]
        public ObjectId PrevObjectId { get; set; }
        
        [RuleObjectIdValid]
        [JsonProperty(Names.NextObjectId)]
        public ObjectId NextObjectId { get; set; }

        public ObjectIdModification()
        {
            PrevObjectId = ObjectId.Null;
            NextObjectId = ObjectId.Null;
        }
        public ObjectIdModification(ObjectId prevObjectId, ObjectId nextObjectId)
        {
            PrevObjectId = prevObjectId;
            NextObjectId = nextObjectId;
        }
        public void Reset()
        {
            PrevObjectId = ObjectId.Null;
            NextObjectId = ObjectId.Null;
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