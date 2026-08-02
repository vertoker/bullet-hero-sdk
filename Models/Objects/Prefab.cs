using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    [DataVersion(DataDomains.Prefab, 1, 0)]
    public class Prefab : IObjectScope, IObjectIdCounter, IFrameLength, IModel<Prefab>
    {
        [RuleIPrimitiveGuidNotNull]
        [JsonProperty(Names.PrefabId)]
        public PrefabId PrefabId { get; set; }

        [RuleNotNull, RuleStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ObjectId)
        // Nested PrefabObject placements (instances of OTHER prefabs, placed inside this template)
        // live directly in here too, already fully materialized - see IObjectScope's own comment.
        [RuleNotNull]
        [JsonProperty(Names.Objects)]
        public Dictionary<ObjectId, RectObject> Objects { get; set; }

        // This prefab's own object-id namespace (mirrors LevelSettings.ObjectIdCounter) - used both
        // to author new objects directly inside this template, and to mint outer ids when this
        // prefab is itself materialized as a nested placement inside another prefab's template.
        [RuleMin(ObjectId.MinLevelValue)]
        [JsonProperty(Names.ObjectIdCounter)]
        public int ObjectIdCounter { get; set; }

        // This template's own local timeline length - has no live placement to derive one from
        // (a template can be referenced by many/zero placements), so it's authored directly,
        // mirroring LevelSettings.FrameLength. Used both as the recommended/default duration for a
        // newly-placed PrefabObject and as the Prefab Timeline's own editing bound.
        [RuleMin(FrameRules.MinFrameLength)]
        [JsonProperty(Names.FrameLengthShort)]
        public int FrameLength { get; set; }

        public ObjectId GetNextObjectId() => new(ObjectIdCounter++);

        public Prefab()
        {
            PrefabId = PrefabId.Null;
            Name = string.Empty;
            Objects = new Dictionary<ObjectId, RectObject>();
            ObjectIdCounter = ObjectId.MinLevelValue;
            FrameLength = PrefabRules.DefaultFrameLength;
        }
        public Prefab(PrefabId prefabId, string name, Dictionary<ObjectId, RectObject> objects, int objectIdCounter, int frameLength)
        {
            PrefabId = prefabId;
            Name = name;
            Objects = objects;
            ObjectIdCounter = objectIdCounter;
            FrameLength = frameLength;
        }
        public void Reset()
        {
            PrefabId = PrefabId.Null;
            Name = string.Empty;
            Objects.Clear();
            ObjectIdCounter = ObjectId.MinLevelValue;
            FrameLength = PrefabRules.DefaultFrameLength;
        }

        public object Clone() => Copy();
        public Prefab Copy() => new(PrefabId, Name, Objects.CopyDictionary(), ObjectIdCounter, FrameLength);

        public override bool Equals(object obj) => obj is Prefab value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(PrefabId, Name,
            Objects.GetDictionaryHashCode(), ObjectIdCounter, FrameLength);

        public bool Equals(Prefab other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = PrefabId.Equals(other.PrefabId)
                         && Name.Equals(other.Name)
                         && Objects.DictionaryEquals(other.Objects)
                         && ObjectIdCounter.Equals(other.ObjectIdCounter)
                         && FrameLength.Equals(other.FrameLength);
            return result;
        }
    }
}