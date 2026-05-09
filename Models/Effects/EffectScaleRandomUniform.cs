using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectScaleRandomUniform : IEffectScale, ICopyable<EffectScaleRandomUniform>
    {
        [JsonProperty(Names.ScaleX)]
        public IVector2 ScaleA { get; set; }
        
        [JsonProperty(Names.ScaleY)]
        public IVector2 ScaleB { get; set; }

        public EffectScaleType GetModelType() => EffectScaleType.RandomUniform;

        public EffectScaleRandomUniform()
        {
            ScaleA = new Vector2Value(
                EffectStatic.Scale_A_X_Default,
                EffectStatic.Scale_A_Y_Default);
            ScaleB = new Vector2Value(
                EffectStatic.Scale_B_X_Default,
                EffectStatic.Scale_B_Y_Default);
        }
        public EffectScaleRandomUniform(IVector2 scaleA, IVector2 scaleB)
        {
            ScaleA = scaleA;
            ScaleB = scaleB;
        }

        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleRandomUniform(ScaleA.Copy(), ScaleB.Copy());
        public EffectScaleRandomUniform Copy() => new(ScaleA.Copy(), ScaleB.Copy());
    }
}