using System;
using BH.SDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BH.SDK.Models.Primitives
{
    public struct ModificationKey : IModel<ModificationKey>, IComparable<ModificationKey>
    {
        [JsonProperty(Names.ObjectId)]
        public ObjectId ObjectId { get; set; }

        [JsonProperty(Names.PathShort)]
        public string Path { get; set; }

        public ModificationKey(ObjectId objectId, string path)
        {
            ObjectId = objectId;
            Path = path;
        }

        public void Reset()
        {
            ObjectId = ObjectId.Null;
            Path = string.Empty;
        }

        public readonly object Clone() => Copy();
        public readonly ModificationKey Copy() => new(ObjectId, Path);

        public readonly bool Equals(ModificationKey other) => ObjectId.Equals(other.ObjectId) && Path == other.Path;
        public readonly override bool Equals(object obj) => obj is ModificationKey other && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(ObjectId, Path);

        public readonly int CompareTo(ModificationKey other)
        {
            var cmp = ObjectId.value.CompareTo(other.ObjectId.value);
            return cmp != 0 ? cmp : string.CompareOrdinal(Path, other.Path);
        }

        public readonly override string ToString() => $"{ObjectId}/{Path}";
    }
}
