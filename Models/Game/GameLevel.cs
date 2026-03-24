using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Events;
using BHSDK.Models.Instances;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameLevel : IInstancesProvider
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
        
        
        [JsonProperty(ModelNames.Instance)]
        public List<Instance> Instances { get; set; }
        
        [JsonProperty(ModelNames.ParentInstance)]
        public List<PrefabInstance> PrefabInstances { get; set; }

        
        [JsonProperty(ModelNames.Theme)]
        public List<Theme> Themes { get; set; }
        
        public GameLevel()
        {
            Seed = 0;
            Framerate = 0;
            ScreenLimit = new ScreenLimitNone();
            
            Events = new GameEvents();
            CameraEvents = new CameraEvents();
            PostProcessingEvents = new PostProcessingEvents();
            PlayerEvents = new PlayerEvents();
            
            Instances = new List<Instance>();
            PrefabInstances = new List<PrefabInstance>();
            
            Themes = new List<Theme>();
        }
        public GameLevel(int seed, int framerate, IScreenLimit screenLimit, GameEvents events, 
            CameraEvents cameraEvents, PostProcessingEvents postProcessingEvents, PlayerEvents playerEvents, 
            List<Instance> instances, List<PrefabInstance> prefabInstances, List<Theme> themes)
        {
            Seed = seed;
            Framerate = framerate;
            ScreenLimit = screenLimit;
            
            Events = events;
            CameraEvents = cameraEvents;
            PostProcessingEvents = postProcessingEvents;
            PlayerEvents = playerEvents;
            
            Instances = instances;
            PrefabInstances = prefabInstances;
            
            Themes = themes;
        }
    }
}