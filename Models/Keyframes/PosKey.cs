using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class PosKey : Keyframe, IModel<PosKey>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinPos, ValueRules.MaxPos)]
        [JsonProperty(Names.Vector2)]
        public IVector2 Pos { get; set; }

        public PosKey()
        {
            Pos = new Vector2Value();
        }
        public PosKey(IVector2 vector2, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Pos = vector2;
        }
        public override void Reset()
        {
            base.Reset();
            Pos = new Vector2Value();
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        PosKey ICopyable<PosKey>.Copy() => CopyImpl();
        
        private PosKey CopyImpl() => new(Pos.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is PosKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Pos);

        public bool Equals(PosKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Pos.Equals(other.Pos);
            return result;
        }
    }
}