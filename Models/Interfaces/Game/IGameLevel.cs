using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Instances;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Game
{
    public interface IGameLevel : IInstancesProvider, IModelReflection<GameLevelV1, IGameLevel>
    {
        public int Seed { get; set; }
        public int Framerate { get; set; }
        
        public List<IMarker> Markers { get; set; }
        public List<ICheckpoint> Checkpoints { get; set; }
        public ICameraData CameraData { get; set; }
    }
}