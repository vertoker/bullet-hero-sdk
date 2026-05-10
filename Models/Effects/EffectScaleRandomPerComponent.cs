using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectScaleRandomPerComponent : IEffectScale, ICopyable<EffectScaleRandomPerComponent>
    {
        [RuleNotNull]
        [JsonProperty(Names.ScaleX)]
        public IVector2 ScaleA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ScaleY)]
        public IVector2 ScaleB { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.RandomPerComponent;

        public EffectScaleRandomPerComponent()
        {
            ScaleA = new Vector2Value(
                EffectRules.Scale.A_X_Default, 
                EffectRules.Scale.A_Y_Default);
            ScaleB = new Vector2Value(
                EffectRules.Scale.B_X_Default, 
                EffectRules.Scale.B_Y_Default);
        }
        public EffectScaleRandomPerComponent(IVector2 scaleA, IVector2 scaleB)
        {
            ScaleA = scaleA;
            ScaleB = scaleB;
        }

        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleRandomPerComponent(ScaleA.Copy(), ScaleB.Copy());
        public EffectScaleRandomPerComponent Copy() => new(ScaleA.Copy(), ScaleB.Copy());
    }
}