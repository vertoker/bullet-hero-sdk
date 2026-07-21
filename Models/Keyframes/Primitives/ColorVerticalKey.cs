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
        [RuleNotNull(typeof(Color4Value))]
        [JsonProperty(Names.ColorBottom)]
        public IColor4 Color4Bottom { get; set; }
        
        [RuleNotNull(typeof(Color4Value))]
        [JsonProperty(Names.ColorTop)]
        public IColor4 Color4Top { get; set; }

        public ColorVerticalKey()
        {
            Color4Bottom = Color4Value.white;
            Color4Top = Color4Value.white;
        }
        public ColorVerticalKey(IColor4 color4, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Color4Bottom = color4.Copy();
            Color4Top = color4.Copy();
        }
        public ColorVerticalKey(IColor4 color4Bottom, IColor4 color4Top, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Color4Bottom = color4Bottom;
            Color4Top = color4Top;
        }
        public override void Reset()
        {
            base.Reset();
            Color4Bottom = Color4Value.white;
            Color4Top = Color4Value.white;
        }
        
        public Color4X4KeyType GetModelType() => Color4X4KeyType.Vertical;
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        ColorVerticalKey ICopyable<ColorVerticalKey>.Copy() => CopyImpl();
        IColor4X4Key ICopyable<IColor4X4Key>.Copy() => CopyImpl();
        
        private ColorVerticalKey CopyImpl() => new(Color4Bottom.Copy(), Color4Top.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is ColorVerticalKey value && Equals(value);
        public bool Equals(IColor4X4Key other) => other is ColorVerticalKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Color4Bottom, Color4Top);

        public bool Equals(ColorVerticalKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Color4Bottom.Equals(other.Color4Bottom)
                         && Color4Top.Equals(other.Color4Top);
            return result;
        }
    }
}