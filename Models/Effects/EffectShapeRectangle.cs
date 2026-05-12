using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapeRectangle : IEffectShape,
        ICopyable<EffectShapeRectangle>, IEquatable<EffectShapeRectangle>
    {
        [RuleNotNull, RuleIVector2Min(EffectRules.Shape.BoxSize_Min)]
        [JsonProperty(Names.Size)]
        public IVector2 Size { get; set; }

        public EffectShapeRectangle()
        {
            Size = new Vector2Value(
                EffectRules.Shape.BoxSize_X_Default,
                EffectRules.Shape.BoxSize_Y_Default);
        }
        public EffectShapeRectangle(IVector2 size)
        {
            Size = size;
        }

        public object Clone() => Copy();
        public EffectShapeType GetModelType() => EffectShapeType.Rectangle;
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeRectangle();
        public EffectShapeRectangle Copy() => new();

        public override bool Equals(object obj) => obj is EffectShapeRectangle value && Equals(value);
        public override int GetHashCode() => Size != null ? Size.GetHashCode() : 0;
        
        public bool Equals(IEffectShape other) => other is EffectShapeRectangle value && Equals(value);
        public bool Equals(EffectShapeRectangle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Size.Equals(other.Size);
            return result;
        }
    }
}