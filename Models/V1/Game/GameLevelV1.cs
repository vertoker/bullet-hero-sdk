using System.Collections.Generic;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.Interfaces.Instances;

namespace BHSDK.Models.V1.Game
{
    public class GameLevelV1 : IGameLevel
    {
        public int Seed { get; set; }
        public int Framerate { get; set; }
        
        public List<IMarker> Markers { get; set; }
        public List<ICheckpoint> Checkpoints { get; set; }
        public ICameraData CameraData { get; set; }
        
        public List<IInstance> Instances { get; set; }
        public List<IPrefabInstance> PrefabInstances { get; set; }

        public GameLevelV1()
        {
            Seed = 0;
            Framerate = 0;
            Markers = new List<IMarker>();
            Checkpoints = new List<ICheckpoint>();
            CameraData = new CameraDataV1();
            
            Instances = new List<IInstance>();
            PrefabInstances = new List<IPrefabInstance>();
        }
        public GameLevelV1(int seed, int framerate, List<IMarker> markers, List<ICheckpoint> checkpoints, 
            ICameraData cameraData, List<IInstance> instances, List<IPrefabInstance> prefabInstances)
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