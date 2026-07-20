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
    public class ColorVerticalKey : Keyframe, IColor4X4Key, IModel<ColorVerticalKey>
    {
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorBottom)]
        public IColor ColorBottom { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorTop)]
        public IColor ColorTop { get; set; }

        public ColorVerticalKey()
        {
            ColorBottom = ColorValue.white;
            ColorTop = ColorValue.white;
        }
        public ColorVerticalKey(IColor color, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorBottom = color.Copy();
            ColorTop = color.Copy();
        }
        public ColorVerticalKey(IColor colorBottom, IColor colorTop, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorBottom = colorBottom;
            ColorTop = colorTop;
        }
        public override void Reset()
        {
            base.Reset();
            ColorBottom = ColorValue.white;
            ColorTop = ColorValue.white;
        }
        
        public Color4X4KeyType GetModelType() => Color4X4KeyType.Vertical;
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        ColorVerticalKey ICopyable<ColorVerticalKey>.Copy() => CopyImpl();
        IColor4X4Key ICopyable<IColor4X4Key>.Copy() => CopyImpl();
        
        private ColorVerticalKey CopyImpl() => new(ColorBottom.Copy(), ColorTop.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is ColorVerticalKey value && Equals(value);
        public bool Equals(IColor4X4Key other) => other is ColorVerticalKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ColorBottom, ColorTop);

        public bool Equals(ColorVerticalKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ColorBottom.Equals(other.ColorBottom)
                         && ColorTop.Equals(other.ColorTop);
            return result;
        }
    }
}