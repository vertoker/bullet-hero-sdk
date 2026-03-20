using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Instances;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameLevel : IInstancesProvider
    {
        [JsonProperty("s")]
        public int Seed { get; set; }
        
        [JsonProperty("fr")]
        public int Framerate { get; set; }
        
        // limitations for screen will be chosen by mappers
        [JsonProperty("sl")]
        public IScreenLimit ScreenLimit { get; set; }
        
        
        [JsonProperty("e")]
        public GameEvents Events { get; set; }
        
        [JsonProperty("ce")]
        public CameraEvents CameraEvents { get; set; }
        
        [JsonProperty("ppe")]
        public PostProcessingEvents PostProcessingEvents { get; set; }
        
        [JsonProperty("pe")]
        public PlayerEvents PlayerEvents { get; set; }
        
        
        [JsonProperty("is")]
        public List<Instance> Instances { get; set; }
        
        [JsonProperty("pis")]
        public List<PrefabInstance> PrefabInstances { get; set; }

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
        }
        public GameLevel(int seed, int framerate, GameEvents gameEvents, CameraEvents cameraEvents, 
            List<Instance> instances, List<PrefabInstance> prefabInstances)
        {
            Seed = seed;
            Framerate = framerate;
            
            Events = gameEvents;
            CameraEvents = cameraEvents;
            
            Instances = instances;
            PrefabInstances = prefabInstances;
        }
    }
}