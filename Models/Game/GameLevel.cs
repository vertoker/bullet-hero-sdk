using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Objects;
using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    [RuleContainer]
    public class GameLevel : IObjectScope, ICopyable<GameLevel>
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
        
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxThemes)]
        [JsonProperty(Names.Themes)]
        public List<Theme> Themes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxPrefabs)]
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
    }
}