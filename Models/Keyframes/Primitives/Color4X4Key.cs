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
    public class Color4X4Key : Keyframe, IColor4X4Key, IModel<Color4X4Key>
    {
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorBL)]
        public IColor ColorBL { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorBR)]
        public IColor ColorBR { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorTL)]
        public IColor ColorTL { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.ColorTR)]
        public IColor ColorTR { get; set; }
        
        public Color4X4Key()
        {
            ColorBL = ColorValue.white;
            ColorBR = ColorValue.white;
            ColorTL = ColorValue.white;
            ColorTR = ColorValue.white;
        }
        public Color4X4Key(IColor value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorBL = value.Copy();
            ColorBR = value.Copy();
            ColorTL = value.Copy();
            ColorTR = value.Copy();
        }
        public Color4X4Key(IColor colorBL, IColor colorBR, IColor colorTL, IColor colorTR,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorBL = colorBL;
            ColorBR = colorBR;
            ColorTL = colorTL;
            ColorTR = colorTR;
        }
        public override void Reset()
        {
            base.Reset();
            ColorBL = ColorValue.white;
            ColorBR = ColorValue.white;
            ColorTL = ColorValue.white;
            ColorTR = ColorValue.white;
        }
        
        public Color4X4KeyType GetModelType() => Color4X4KeyType.BariCentrical;
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        Color4X4Key ICopyable<Color4X4Key>.Copy() => CopyImpl();
        IColor4X4Key ICopyable<IColor4X4Key>.Copy() => CopyImpl();
        
        private Color4X4Key CopyImpl() => new(ColorBL.Copy(), ColorBR.Copy(), ColorTL.Copy(), ColorTR.Copy(), Frame, Ease);
        
        public override bool Equals(object obj) => obj is Color4X4Key value && Equals(value);
        public bool Equals(IColor4X4Key other) => other is Color4X4Key value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ColorBL, ColorBR, ColorTL, ColorTR);

        public bool Equals(Color4X4Key other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ColorBL.Equals(other.ColorBL)
                         && ColorBR.Equals(other.ColorBR)
                         && ColorTL.Equals(other.ColorTL)
                         && ColorTR.Equals(other.ColorTR);
            return result;
        }
    }
}