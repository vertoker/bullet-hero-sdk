using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Events
{
    [RuleContainer]
    public class Checkpoint : IFrame, IModel<Checkpoint>
    {
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.ActiveShort)]
        public bool Active { get; set; }
        
        [RuleNotNull(typeof(Color4Value))]
        [JsonProperty(Names.Color)]
        public IColor4 Color4 { get; set; }

        public Checkpoint()
        {
            Frame = FrameRules.MinFrame;
            Name = string.Empty;
            Active = true;
            Color4 = Color4Value.white;
        }
        public Checkpoint(string name, bool active, IColor4 color4, int frame)
        {
            Frame = frame;
            Name = name;
            Active = active;
            Color4 = color4;
        }
        public void Reset()
        {
            Frame = FrameRules.MinFrame;
            Name = string.Empty;
            Active = true;
            Color4 = Color4Value.white;
        }

        public object Clone() => Copy();
        public Checkpoint Copy() => new(Name, Active, Color4.Copy(), Frame);

        public override bool Equals(object obj) => obj is Checkpoint value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, Name, Active, Color4);

        public bool Equals(Checkpoint other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Frame.Equals(other.Frame)
                         && Name.Equals(other.Name)
                         && Active == other.Active
                         && Color4.Equals(other.Color4);
            return result;
        }
    }
}