using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Game
{
    public interface IGameLevel : IModelReflection<GameLevelV1, IGameLevel>
    {
        public int Seed { get; set; }
        public int Framerate { get; set; }
        
        public List<IMarker> Markers { get; set; }
        public List<ICheckpoint> Checkpoints { get; set; }
        public List<ILevelObject> Objects { get; set; }
        public ICameraData CameraData { get; set; }
    }
}