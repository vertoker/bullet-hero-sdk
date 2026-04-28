using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectShapeTorus : IEffectShape, ICopyable<EffectShapeTorus>
    {
        [JsonProperty(Names.RadiusMajor)]
        public IFloat RadiusMinor { get; set; }
        
        [JsonProperty(Names.RadiusMinor)]
        public IFloat RadiusMajor { get; set; }
        
        [JsonProperty(Names.Arc)]
        public IFloat Arc { get; set; }
        
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Torus;
        
        public EffectShapeTorus()
        {
            RadiusMinor = new FloatValue(EffectStatic.Shape_TorusRadiusMinorDefault);
            RadiusMajor = new FloatValue(EffectStatic.Shape_TorusRadiusMajorDefault);
            Arc = new FloatValue(EffectStatic.Shape_ArcDefault);
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