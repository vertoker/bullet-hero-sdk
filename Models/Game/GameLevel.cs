using System.Collections.Generic;
using BHSDK.Models.Instances;
using BHSDK.Models.Interfaces.Instances;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameLevel : IInstancesProvider
    {
        [JsonProperty("s")]
        public int Seed { get; set; }
        
        [JsonProperty("fr")]
        public int Framerate { get; set; }
        
        
        [JsonProperty("ms")]
        public List<Marker> Markers { get; set; }
        
        [JsonProperty("cs")]
        public List<Checkpoint> Checkpoints { get; set; }
        
        [JsonProperty("cd")]
        public CameraData CameraData { get; set; }
        
        
        [JsonProperty("is")]
        public List<IInstance> Instances { get; set; }
        
        [JsonProperty("pis")]
        public List<PrefabInstance> PrefabInstances { get; set; }

        public GameLevel()
        {
            Seed = 0;
            Framerate = 0;
            Markers = new List<Marker>();
            Checkpoints = new List<Checkpoint>();
            CameraData = new CameraData();
            
            Instances = new List<IInstance>();
            PrefabInstances = new List<PrefabInstance>();
        }
        public GameLevel(int seed, int framerate, List<Marker> markers, List<Checkpoint> checkpoints, 
            CameraData cameraData, List<IInstance> instances, List<PrefabInstance> prefabInstances)
        {
            Seed = seed;
            Framerate = framerate;
            Markers = markers;
            Checkpoints = checkpoints;
            CameraData = cameraData;
            
            Instances = instances;
            PrefabInstances = prefabInstances;
        }
    }
}