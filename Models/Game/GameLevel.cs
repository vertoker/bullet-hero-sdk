using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;
using Object = BH.SDK.Models.Objects.Object;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    public class GameLevel : IObjectScope,
        ICopyable<GameLevel>, IEquatable<GameLevel>
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
        [RuleCollectionUnique(nameof(Object.ObjectId))]
        [JsonProperty(Names.Objects)]
        public List<Object> Objects { get; set; }
        
        // TODO add more contextual checks
        [RuleNotNull]
        [JsonProperty(Names.ParentObjects)]
        public List<PrefabObject> PrefabObjects { get; set; }
        
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxThemes)]
        [JsonProperty(Names.Themes)]
        public List<Theme> Themes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPrefabs)]
        [RuleCollectionUnique(nameof(PrefabObject.PrefabGuid))]
        [JsonProperty(Names.Prefabs)]
        public List<Prefab> Prefabs { get; set; }
        
        public GameLevel()
        {
            Events = new GameEvents();
            CameraEvents = new CameraEvents();
            PostProcessingEvents = new PostProcessingEvents();
            PlayerEvents = new PlayerEvents();
            
            Objects = new List<Object>();
            PrefabObjects = new List<PrefabObject>();
            
            Prefabs = new List<Prefab>();
            Themes = new List<Theme>();
        }
        public GameLevel(GameEvents events, CameraEvents cameraEvents, PostProcessingEvents postProcessingEvents,
            PlayerEvents playerEvents, List<Object> objects, List<PrefabObject> prefabObjects,
            List<Prefab> prefabs, List<Theme> themes)
        {
            Events = events;
            CameraEvents = cameraEvents;
            PostProcessingEvents = postProcessingEvents;
            PlayerEvents = playerEvents;
            
            Objects = objects;
            PrefabObjects = prefabObjects;
            
            Prefabs = prefabs;
            Themes = themes;
        }

        public object Clone() => Copy();
        public GameLevel Copy() => new(Events.Copy(), CameraEvents.Copy(),
            PostProcessingEvents.Copy(), PlayerEvents.Copy(),
            Objects.CopyList(), PrefabObjects.CopyList(),
            Prefabs.CopyList(), Themes.CopyList());

        public bool Equals(GameLevel other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Events.Equals(other.Events) 
                         && CameraEvents.Equals(other.CameraEvents)
                         && PostProcessingEvents.Equals(other.PostProcessingEvents)
                         && PlayerEvents.Equals(other.PlayerEvents)
                         && Objects.ListEquals(other.Objects)
                         && PrefabObjects.ListEquals(other.PrefabObjects)
                         && Themes.ListEquals(other.Themes)
                         && Prefabs.ListEquals(other.Prefabs);
            return result;
        }

        public override bool Equals(object obj) => obj is GameLevel value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Events, CameraEvents, PostProcessingEvents, PlayerEvents,
            Objects.GetListHashCode(), PrefabObjects.GetListHashCode(), Themes.GetListHashCode(), Prefabs.GetListHashCode());
    }
}