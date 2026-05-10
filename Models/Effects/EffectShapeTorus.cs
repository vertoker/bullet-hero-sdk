using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectShapeTorus : IEffectShape, ICopyable<EffectShapeTorus>
    {
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.TorusRadiusMinor_Min)]
        [JsonProperty(Names.RadiusMinor)]
        public IFloat RadiusMinor { get; set; }
        
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.TorusRadiusMajor_Min)]
        [JsonProperty(Names.RadiusMajor)]
        public IFloat RadiusMajor { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.Arc_Min, EffectRules.Shape.Arc_Max)]
        [JsonProperty(Names.Arc)]
        public IFloat Arc { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Torus;
        
        public EffectShapeTorus()
        {
            RadiusMinor = new FloatValue(EffectRules.Shape.TorusRadiusMinor_Default);
            RadiusMajor = new FloatValue(EffectRules.Shape.TorusRadiusMajor_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeTorus(float radiusMinor, float radiusMajor, float arc, IEffectShapeSpread spread)
        {
            RadiusMinor = new FloatValue(radiusMinor);
            RadiusMajor = new FloatValue(radiusMajor);
            Arc = new FloatValue(arc);
            Spread = spread;
        }
        public EffectShapeTorus(IFloat radiusMinor, IFloat radiusMajor, IFloat arc, IEffectShapeSpread spread)
        {
            RadiusMinor = radiusMinor;
            RadiusMajor = radiusMajor;
            Arc = arc;
            Spread = spread;
        }

        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeTorus(RadiusMinor.Copy(), RadiusMajor.Copy(), Arc.Copy(), Spread.Copy());
        public EffectShapeTorus Copy() => new(RadiusMinor.Copy(), RadiusMajor.Copy(), Arc.Copy(), Spread.Copy());
    }
}