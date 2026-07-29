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
        [RuleObjectIdValid]
        [JsonProperty(Names.ObjectId)]
        public ObjectId ObjectId { get; set; } // to which Object this modification is applied, means prevObjectId

        [RuleNotNull]
        [JsonProperty(Names.PathShort)]
        public string Path { get; set; }

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
            ObjectId = ObjectId.Null;
            Path = string.Empty;
            Value = null;
        }
        public Modification(ObjectId objectId, string path, object value)
        {
            ObjectId = objectId;
            Path = path;
            Value = value;
        }
        public void Reset()
        {
            ObjectId = ObjectId.Null;
            Path = string.Empty;
            Value = null;
        }

        public object Clone() => Copy();
        public Modification Copy() => new(ObjectId, Path, CopyValue());

        public object CopyValue()
        {
            if (Value == null) return null;
            if (Value.GetType().IsValueType) return Value;
            if (Value is ICloneable cloneable) return cloneable.Clone();
            return Value;
        }

        public override bool Equals(object obj) => obj is Modification value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ObjectId, Path, Value);

        public bool Equals(Modification other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ObjectId.Equals(other.ObjectId)
                         && Path.Equals(other.Path)
                         && Value.Equals(other.Value);
            return result;
        }
    }
}