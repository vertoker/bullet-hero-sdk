using System;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Data
{
    [RuleContainer]
    [DataVersion(DataDomains.EffectData, 1, 0)]
    public class EffectData : IModel<EffectData>, IUpdatable<EffectData>
    {
        [RuleIPrimitiveGuidNotNull]
        [JsonProperty(Names.EffectId)]
        public EffectId EffectId { get; set; }

        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.HasStopLocalFrame)]
        public bool HasStopLocalFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.StopLocalFrame)]
        public int StopLocalFrame { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Core)]
        public EffectObjectCore Core { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Forces)]
        public EffectObjectForces Forces { get; set; }
        
        [RuleNotNull(typeof(EffectShapePoint))]
        [JsonProperty(Names.Shape)]
        public IEffectShape Shape { get; set; }
        
        [RuleNotNull(typeof(EffectAngleValue))]
        [JsonProperty(Names.Angle)]
        public IEffectAngle Angle { get; set; }
        
        [RuleNotNull(typeof(EffectScaleValue))]
        [JsonProperty(Names.Scale)]
        public IEffectScale Scale { get; set; }
        
        [RuleNotNull(typeof(EffectColorValue))]
        [JsonProperty(Names.Color)]
        public IEffectColor Color { get; set; }

        public EffectData()
        {
            EffectId = EffectId.Null;
            Name = string.Empty;
            HasStopLocalFrame = EffectRules.HasStopLocalFrame_Default;
            StopLocalFrame = EffectRules.StopLocalFrame_Default;
            Core = new EffectObjectCore();
            Forces = new EffectObjectForces();
            Shape = new EffectShapePoint();
            Angle = new EffectAngleValue();
            Scale = new EffectScaleValue();
            Color = new EffectColorValue();
        }
        public EffectData(EffectId effectId, string name, bool hasStopLocalFrame, int stopLocalFrame, EffectObjectCore core,
            EffectObjectForces forces, IEffectShape shape, IEffectAngle angle, IEffectScale scale, IEffectColor color)
        {
            EffectId = effectId;
            Name = name;
            HasStopLocalFrame = hasStopLocalFrame;
            StopLocalFrame = stopLocalFrame;
            Core = core;
            Forces = forces;
            Shape = shape;
            Angle = angle;
            Scale = scale;
            Color = color;
        }
        public void Reset()
        {
            EffectId = EffectId.Null;
            Name = string.Empty;
            HasStopLocalFrame = EffectRules.HasStopLocalFrame_Default;
            StopLocalFrame = EffectRules.StopLocalFrame_Default;
            Core.Reset();
            Forces.Reset();
            Shape = new EffectShapePoint();
            Angle = new EffectAngleValue();
            Scale = new EffectScaleValue();
            Color = new EffectColorValue();
        }

        public object Clone() => CopyImpl();
        public EffectData Copy() => CopyImpl();
        
        private EffectData CopyImpl() => new(EffectId, Name, HasStopLocalFrame, StopLocalFrame, Core.Copy(),
            Forces.Copy(), Shape.Copy(), Angle.Copy(), Scale.Copy(), Color.Copy());
        
        public void Update(EffectData src)
        {
            EffectId = src.EffectId;
            Name = src.Name;
            HasStopLocalFrame = src.HasStopLocalFrame;
            StopLocalFrame = src.StopLocalFrame;
            Core.Update(src.Core);
            Forces.Update(src.Forces);
            Shape = src.Shape.Copy();
            Angle = src.Angle.Copy();
            Scale = src.Scale.Copy();
            Color = src.Color.Copy();
        }

        public override bool Equals(object obj) => obj is EffectData value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(EffectId);
            hashCode.Add(HasStopLocalFrame);
            hashCode.Add(StopLocalFrame);
            hashCode.Add(Core);
            hashCode.Add(Forces);
            hashCode.Add(Shape);
            hashCode.Add(Angle);
            hashCode.Add(Scale);
            hashCode.Add(Color);
            return hashCode.ToHashCode();
        }
        
        public bool Equals(EffectData other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsEffectData(other);
            return result;
        }
        
        private bool EqualsEffectData(EffectData other)
        {
            var result = EffectId.Equals(other.EffectId)
                         && Name.Equals(other.Name)
                         && HasStopLocalFrame == other.HasStopLocalFrame
                         && StopLocalFrame.Equals(other.StopLocalFrame)
                         && Core.Equals(other.Core)
                         && Forces.Equals(other.Forces)
                         && Shape.Equals(other.Shape)
                         && Angle.Equals(other.Angle)
                         && Scale.Equals(other.Scale)
                         && Color.Equals(other.Color);
            return result;
        }
    }
}