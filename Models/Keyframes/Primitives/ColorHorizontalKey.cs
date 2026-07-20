using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Enum.Keyframes;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class ColorHorizontalKey : Keyframe, IColor4X4Key, IModel<ColorHorizontalKey>
    {
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorLeft)]
        public IColor ColorLeft { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorRight)]
        public IColor ColorRight { get; set; }

        public ColorHorizontalKey()
        {
            ColorLeft = ColorValue.white;
            ColorRight = ColorValue.white;
        }
        public ColorHorizontalKey(IColor color, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorLeft = color.Copy();
            ColorRight = color.Copy();
        }
        public ColorHorizontalKey(IColor colorLeft, IColor colorRight, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorLeft = colorLeft;
            ColorRight = colorRight;
        }
        public override void Reset()
        {
            base.Reset();
            ColorLeft = ColorValue.white;
            ColorRight = ColorValue.white;
        }
        
        public Color4X4KeyType GetModelType() => Color4X4KeyType.Horizontal;
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        ColorHorizontalKey ICopyable<ColorHorizontalKey>.Copy() => CopyImpl();
        IColor4X4Key ICopyable<IColor4X4Key>.Copy() => CopyImpl();
        
        private ColorHorizontalKey CopyImpl() => new(ColorLeft.Copy(), ColorRight.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is ColorHorizontalKey value && Equals(value);
        public bool Equals(IColor4X4Key other) => other is ColorHorizontalKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ColorLeft, ColorRight);

        public bool Equals(ColorHorizontalKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ColorLeft.Equals(other.ColorLeft)
                         && ColorRight.Equals(other.ColorRight);
            return result;
        }
    }
}