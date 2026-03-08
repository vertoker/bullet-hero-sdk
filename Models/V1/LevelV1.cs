using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.V1
{
    public class LevelV1 : ILevel
    {
        public ILevelMeta Meta { get; set; } = new LevelMetaV1();
        public ILevelTrack Track { get; set; } = new LevelTrackV1();
        public IGameLevel Game { get; set; } = new GameLevelV1();
    }
}