using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Events;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Objects;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameLevel : IObjectScope
    {
        [JsonProperty(ModelNames.Seed)]
        public int Seed { get; set; }
        
        [JsonProperty(ModelNames.Framerate)]
        public int Framerate { get; set; }
        
        // limitations for screen will be chosen by mappers
        [JsonProperty(ModelNames.Screen + ModelNames.Limit)]
        public IScreenLimit ScreenLimit { get; set; }
        
        
        [JsonProperty(ModelNames.Event)]
        public GameEvents Events { get; set; }
        
        [JsonProperty(ModelNames.Camera + ModelNames.Event)]
        public CameraEvents CameraEvents { get; set; }
        
        [JsonProperty(ModelNames.PostProcessing + ModelNames.Event)]
        public PostProcessingEvents PostProcessingEvents { get; set; }
        
        [JsonProperty(ModelNames.Player + ModelNames.Event)]
        public PlayerEvents PlayerEvents { get; set; }
        
        
        [JsonProperty(ModelNames.Object)]
        public List<Object> Objects { get; set; }
        
        [JsonProperty(ModelNames.ParentObject)]
        public List<PrefabObject> PrefabObjects { get; set; }
        
        
        [JsonProperty(ModelNames.Prefab)]
        public List<Prefab> Prefabs { get; set; }
        
        [JsonProperty(ModelNames.Theme)]
        public List<Theme> Themes { get; set; }
        
        public GameLevel()
        {
            Seed = 0;
            Framerate = 60;
            ScreenLimit = new ScreenLimitNone();
            
            Events = new GameEvents();
            CameraEvents = new CameraEvents();
            PostProcessingEvents = new PostProcessingEvents();
            PlayerEvents = new PlayerEvents();
            
            Objects = new List<Object>();
            PrefabObjects = new List<PrefabObject>();
            
            Prefabs = new List<Prefab>();
            Themes = new List<Theme>();
        }
        public GameLevel(int seed, int framerate, IScreenLimit screenLimit, GameEvents events, 
            CameraEvents cameraEvents, PostProcessingEvents postProcessingEvents, PlayerEvents playerEvents, 
            List<Object> objects, List<PrefabObject> prefabObjects, List<Prefab> prefabs, List<Theme> themes)
        {
            Seed = seed;
            Framerate = framerate;
            ScreenLimit = screenLimit;
            
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