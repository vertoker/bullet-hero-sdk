using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class AngleKey : Keyframe, IModel<AngleKey>
    {
        [RuleNotNull(typeof(FloatValue))]
        [JsonProperty(Names.Float)]
        public IFloat Angle { get; set; }

        public AngleKey()
        {
            Angle = new FloatValue();
        }
        public AngleKey(IFloat value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Angle = value;
        }
        public override void Reset()
        {
            base.Reset();
            Angle = new FloatValue();
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        AngleKey ICopyable<AngleKey>.Copy() => CopyImpl();
        
        private AngleKey CopyImpl() => new(Angle.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is AngleKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Angle);

        public bool Equals(AngleKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Angle.Equals(other.Angle);
            return result;
        }
    }
}