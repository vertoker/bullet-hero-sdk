using System.Collections.Generic;
using BHSDK.Models.Interfaces.Game;

namespace BHSDK.Models.V1.Game
{
    public class GameLevelV1 : IGameLevel
    {
        public int Seed { get; set; }
        public int Framerate { get; set; }
        
        public List<IMarker> Markers { get; set; }
        public List<ICheckpoint> Checkpoints { get; set; }
        public List<ILevelObject> Objects { get; set; }
        public ICameraData CameraData { get; set; }
    }
}