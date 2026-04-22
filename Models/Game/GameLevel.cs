using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Objects;
using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameLevel : IObjectScope
    {
        [JsonProperty(Names.Events)]
        public GameEvents Events { get; set; }
        
        [JsonProperty(Names.CameraEvents)]
        public CameraEvents CameraEvents { get; set; }
        
        [JsonProperty(Names.PostProcessingEvents)]
        public PostProcessingEvents PostProcessingEvents { get; set; }
        
        [JsonProperty(Names.PlayerEvents)]
        public PlayerEvents PlayerEvents { get; set; }
        
        
        [JsonProperty(Names.Objects)]
        public List<Object> Objects { get; set; }
        
        [JsonProperty(Names.ParentObjects)]
        public List<PrefabObject> PrefabObjects { get; set; }
        
        
        [JsonProperty(Names.Prefabs)]
        public List<Prefab> Prefabs { get; set; }
        
        [JsonProperty(Names.Themes)]
        public List<Theme> Themes { get; set; }
        
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
    }
}