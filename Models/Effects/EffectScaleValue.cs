using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectScaleValue : IEffectScale, ICopyable<EffectScaleValue>
    {
        [JsonProperty(Names.Scale)]
        public IVector2 Scale { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.Value;

        public EffectScaleValue()
        {
            Scale = new Vector2Value(
                EffectStatic.Scale_A_X_Default,
                EffectStatic.Scale_A_Y_Default);
        }
        public EffectScaleValue(IVector2 scale)
        {
            Scale = scale;
        }

        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleValue(Scale.Copy());
        public EffectScaleValue Copy() => new(Scale.Copy());
    }
}