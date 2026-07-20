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
    public class LayerKey : Keyframe, IModel<LayerKey>
    {
        [RuleNotNull(typeof(IntValue)), RuleIIntInRange(ValueRules.MinLayer, ValueRules.MaxLayer)]
        [JsonProperty(Names.Int)]
        public IInt Layer { get; set; }

        public LayerKey()
        {
            Layer = new IntValue();
        }
        public LayerKey(IInt value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Layer = value;
        }
        public override void Reset()
        {
            base.Reset();
            Layer = new IntValue();
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        LayerKey ICopyable<LayerKey>.Copy() => CopyImpl();
        
        private LayerKey CopyImpl() => new(Layer.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is LayerKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Layer);

        public bool Equals(LayerKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Layer.Equals(other.Layer);
            return result;
        }
    }
}