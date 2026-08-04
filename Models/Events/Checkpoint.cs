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
    /// <summary>
    /// A point playback rewinds to after a death. Real gameplay state, unlike the purely decorative
    /// Marker it sits next to - place them and a hard section stops costing the whole run.
    /// </summary>
    [RuleContainer]
    public class Checkpoint : IFrame, IModel<Checkpoint>
    {
        /// <summary> Level frame a retry resumes from. </summary>
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }

        /// <summary> Editor-facing label. </summary>
        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }

        /// <summary> Whether the checkpoint counts - lets a mapper disable one without deleting it
        /// and losing its position. </summary>
        [JsonProperty(Names.ActiveShort)]
        public bool Active { get; set; }

        /// <summary> Color of the in-game checkpoint marker; themeable, unlike Marker's. </summary>
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