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

        public GameLevelV1()
        {
            Seed = 0;
            Framerate = 0;
            Markers = new List<IMarker>();
            Checkpoints = new List<ICheckpoint>();
            Objects = new List<ILevelObject>();
            CameraData = new CameraDataV1();
        }
        public GameLevelV1(int seed, int framerate, List<IMarker> markers, 
            List<ICheckpoint> checkpoints, List<ILevelObject> objects, ICameraData cameraData)
        {
            Seed = seed;
            Framerate = framerate;
            Markers = markers;
            Checkpoints = checkpoints;
            Objects = objects;
            CameraData = cameraData;
        }
    }
}