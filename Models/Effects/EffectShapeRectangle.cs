using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectShapeRectangle : IEffectShape, ICopyable<EffectShapeRectangle>
    {
        [JsonProperty(Names.Size)]
        public IVector2 Size { get; set; }

        public EffectShapeRectangle()
        {
            Size = new Vector2Value(
                EffectStatic.Shape_BoxSize_X_Default,
                EffectStatic.Shape_BoxSize_Y_Default);
        }
        public EffectShapeRectangle(IVector2 size)
        {
            Size = size;
        }

        public EffectShapeType GetModelType() => EffectShapeType.Rectangle;
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeRectangle();
        public EffectShapeRectangle Copy() => new();
    }
}