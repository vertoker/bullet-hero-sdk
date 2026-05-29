using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class Object : ICopyable<Object>, IEquatable<Object>, IUpdatable<Object>
    {
        public virtual ObjectType GetModelType() => ObjectType.Object;
        
        [RuleObjectIdValid]
        [JsonProperty(Names.ObjectId)]
        public int ObjectId { get; set; }
        
        [RuleMin(IdRules.MinParentObjectId, IdRules.NullObjectId)]
        [JsonProperty(Names.ParentObjectId)]
        public int ParentObjectId { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.VisibleShort)]
        public bool Visible { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.StartFrameShort)]
        public int StartFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.EndFrameShort)]
        public int EndFrame { get; set; }

        public Object()
        {
            ObjectId = IdRules.NullObjectId;
            ParentObjectId = IdRules.NullObjectId;
            Name = string.Empty;
            Visible = true;
            StartFrame = FrameRules.MinFrame;
            EndFrame = FrameRules.MinFrame;
        }
        public Object(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame)
        {
            ObjectId = objectId;
            ParentObjectId = parentObjectId;
            Name = name;
            Visible = visible;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }

        public virtual object Clone() => CopyImpl();
        public virtual Object Copy() => CopyImpl();
        
        private Object CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame);
        
        public void Update(Object src)
        {
            ObjectId = src.ObjectId;
            ParentObjectId = src.ParentObjectId;
            Name = src.Name;
            Visible = src.Visible;
            StartFrame = src.StartFrame;
            EndFrame = src.EndFrame;
        }

        public override bool Equals(object obj) => obj is Object value && Equals(value);
        public override int GetHashCode() =>
            HashCode.Combine(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame);

        public virtual bool Equals(Object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other);
            return result;
        }
        
        protected bool EqualsObject(Object other)
        {
            var result = ObjectId.Equals(other.ObjectId)
                         && ParentObjectId.Equals(other.ParentObjectId)
                         && Name.Equals(other.Name)
                         && Visible == other.Visible
                         && StartFrame.Equals(other.StartFrame)
                         && EndFrame.Equals(other.EndFrame);
            return result;
        }
    }
}