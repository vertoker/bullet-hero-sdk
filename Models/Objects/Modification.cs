using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    // Limitations for modifications

    // 1. Works only for RectObject and Prefab, not applied to anything else

    // 2. You can't make RectObject a child or any Object in Prefab, no parenting from low levels
    // (but you still can make root of prefab inherit from outside Object, parenting from high levels is allowed)

    // 3. Modification works only for prefab scope where it's located.
    // No deep inheritance of changes

    [RuleContainer]
    public class Modification : IModel<Modification>
    {
        // WHICH object (inner/template ObjectId) and WHICH field (Path) this override applies to -
        // see ModificationKey's own doc comment. Also PrefabObject.Modifications' dictionary key.
        [JsonProperty(Names.Key)]
        public ModificationKey Key { get; set; }

        private object _value;

        [RuleNotNull]
        [JsonProperty(Names.ValueShort)]
        public object Value
        {
            get => _value;
            // Widened to the same CLR types Newtonsoft always produces when deserializing a raw
            // JSON number into an `object` property (long for any integral, double for any
            // floating-point) - without this, a Modification built in code with e.g. a plain `int`
            // stops Equals-ing itself after a serialize/deserialize round trip, since the
            // deserialized copy always comes back as long/double regardless of the original width.
            set => _value = NormalizeValue(value);
        }

        private static object NormalizeValue(object value) => value switch
        {
            sbyte or byte or short or ushort or int or uint or long or ulong => Convert.ToInt64(value),
            float or double or decimal => Convert.ToDouble(value),
            _ => value,
        };

        public Modification()
        {
            Key = new ModificationKey(ObjectId.Null, string.Empty);
            Value = null;
        }
        public Modification(ObjectId objectId, string path, object value)
        {
            Key = new ModificationKey(objectId, path);
            Value = value;
        }
        public Modification(ModificationKey key, object value)
        {
            Key = key;
            Value = value;
        }
        public void Reset()
        {
            var key = Key;
            key.Reset();
            Key = key;
            Value = null;
        }

        public object Clone() => Copy();
        public Modification Copy() => new(Key.Copy(), CopyValue());

        public object CopyValue()
        {
            if (Value == null) return null;
            if (Value.GetType().IsValueType) return Value;
            if (Value is ICloneable cloneable) return cloneable.Clone();
            return Value;
        }

        public override bool Equals(object obj) => obj is Modification value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Key, Value);

        public bool Equals(Modification other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Key.Equals(other.Key)
                         && Value.Equals(other.Value);
            return result;
        }
    }
}