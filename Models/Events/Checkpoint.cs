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
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }

        public Checkpoint()
        {
            Frame = FrameRules.MinFrame;
            Name = string.Empty;
            Active = true;
            Color = ColorValue.white;
        }
        public Checkpoint(string name, bool active, IColor color, int frame)
        {
            Frame = frame;
            Name = name;
            Active = active;
            Color = color;
        }
        public void Reset()
        {
            Frame = FrameRules.MinFrame;
            Name = string.Empty;
            Active = true;
            Color = ColorValue.white;
        }

        public object Clone() => Copy();
        public Checkpoint Copy() => new(Name, Active, Color.Copy(), Frame);

        public override bool Equals(object obj) => obj is Checkpoint value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, Name, Active, Color);

        public bool Equals(Checkpoint other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Frame.Equals(other.Frame)
                         && Name.Equals(other.Name)
                         && Active == other.Active
                         && Color.Equals(other.Color);
            return result;
        }
    }
}