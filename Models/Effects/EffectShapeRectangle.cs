using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeRectangle : IEffectShape, ICopyable<EffectShapeRectangle>
    {
        [JsonProperty(Names.Size)]
        public IVector2 Size { get; set; }

        public EffectShapeRectangle()
        {
            Size = new Vector2Value(EffectStatic.Shape_BoxSizeDefault);
        }
        public EffectShapeRectangle(Vector2 size)
        {
            Size = new Vector2Value(size);
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