using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    public class GameLevel : IObjectScope, ICopyable<GameLevel>, IEquatable<GameLevel>
    {
        [RuleNotNull]
        [JsonProperty(Names.Events)]
        public GameEvents Events { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.CameraEvents)]
        public CameraEvents CameraEvents { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.PostProcessingEvents)]
        public PostProcessingEvents PostProcessingEvents { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.PlayerEvents)]
        public PlayerEvents PlayerEvents { get; set; }
        
        // TODO add more contextual checks
        [RuleNotNull]
        [JsonProperty(Names.Objects)]
        public Dictionary<ObjectId, RectObject> Objects { get; set; }
        
        // TODO add more contextual checks
        [RuleNotNull]
        [JsonProperty(Names.ParentObjects)]
        public List<PrefabObject> PrefabObjects { get; set; }
        
        public GameLevel()
        {
            Events = new GameEvents();
            CameraEvents = new CameraEvents();
            PostProcessingEvents = new PostProcessingEvents();
            PlayerEvents = new PlayerEvents();
            
            Objects = new Dictionary<ObjectId, RectObject>();
            PrefabObjects = new List<PrefabObject>();
        }
        public GameLevel(GameEvents events, CameraEvents cameraEvents, PostProcessingEvents postProcessingEvents,
            PlayerEvents playerEvents, Dictionary<ObjectId, RectObject> objects, List<PrefabObject> prefabObjects)
        {
            Events = events;
            CameraEvents = cameraEvents;
            PostProcessingEvents = postProcessingEvents;
            PlayerEvents = playerEvents;
            
            Objects = objects;
            PrefabObjects = prefabObjects;
        }

        public object Clone() => Copy();
        public GameLevel Copy() => new(Events.Copy(), CameraEvents.Copy(), PostProcessingEvents.Copy(),
            PlayerEvents.Copy(), Objects.CopyDictionary(), PrefabObjects.CopyList());

        public override bool Equals(object obj) => obj is GameLevel value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Events, CameraEvents, PostProcessingEvents, PlayerEvents,
            Objects.GetDictionaryHashCode(), PrefabObjects.GetListHashCode());

        public bool Equals(GameLevel other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Events.Equals(other.Events) 
                         && CameraEvents.Equals(other.CameraEvents)
                         && PostProcessingEvents.Equals(other.PostProcessingEvents)
                         && PlayerEvents.Equals(other.PlayerEvents)
                         && Objects.DictionaryEquals(other.Objects)
                         && PrefabObjects.ListEquals(other.PrefabObjects);
            return result;
        }
    }
}