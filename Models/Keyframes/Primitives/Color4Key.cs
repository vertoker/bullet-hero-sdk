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
    public class Color4Key : Keyframe, IColor4X4Key, IModel<Color4Key>
    {
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Value { get; set; }

        public Color4Key()
        {
            Value = ColorValue.white;
        }
        public Color4Key(IColor value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }
        public override void Reset()
        {
            base.Reset();
            Value = ColorValue.white;
        }
        
        public Color4X4KeyType GetModelType() => Color4X4KeyType.Value;
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        Color4Key ICopyable<Color4Key>.Copy() => CopyImpl();
        IColor4X4Key ICopyable<IColor4X4Key>.Copy() => CopyImpl();
        
        private Color4Key CopyImpl() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Color4Key value && Equals(value);
        public bool Equals(IColor4X4Key other) => other is Color4Key value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(Color4Key other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Value.Equals(other.Value);
            return result;
        }
    }
}