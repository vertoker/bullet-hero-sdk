using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    // Limitations for modifications

    // 1. Works only for Object and Prefab, not applied to anything else

    // 2. You can't make Object a child or any Object in Prefab, no parenting from low levels
    // (but you still can make root of prefab inherit from outside Object, parenting from high levels is allowed)

    // 3. Modification works only for prefab scope where it's located.
    // No deep inheritance of changes

    public class Modification
    {
        [JsonProperty(Names.ObjectId)]
        public int ObjectId { get; set; } // to which Object this modification is applied, means prevObjectId

        [JsonProperty(Names.PathShort)]
        public ObjectPropertyPath Path { get; set; }

        [JsonProperty(Names.Index)]
        public int Index { get; set; }

        [JsonProperty(Names.ValueShort)]
        public object Value { get; set; }

        public Modification()
        {
            ObjectId = 0;
            Path = ObjectPropertyPath.None;
            Index = 0;
            Value = null;
        }
        public Modification(int objectId, ObjectPropertyPath path, int index, object value)
        {
            ObjectId = objectId;
            Path = path;
            Index = index;
            Value = value;
        }
    }
}