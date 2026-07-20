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
    public class FloatKey : Keyframe, IModel<FloatKey>
    {
        [RuleNotNull(typeof(FloatValue))]
        [JsonProperty(Names.Float)]
        public IFloat Value { get; set; }

        public FloatKey()
        {
            Value = new FloatValue();
        }
        public FloatKey(IFloat value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }
        public override void Reset()
        {
            base.Reset();
            Value = new FloatValue();
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        FloatKey ICopyable<FloatKey>.Copy() => CopyImpl();
        
        private FloatKey CopyImpl() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is FloatKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(FloatKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Value.Equals(other.Value);
            return result;
        }
    }
}