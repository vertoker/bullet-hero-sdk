using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.V1
{
    public class LevelV1 : ILevel
    {
        public ILevelMeta Meta { get; set; }
        public ILevelTrack Track { get; set; }
        public ILevelGameplay Gameplay { get; set; }
        public IGameLevel Game { get; set; }

        public LevelV1()
        {
            Meta = new LevelMetaV1();
            Track = new LevelTrackV1();
            Gameplay = new LevelGameplayV1();
            Game = new GameLevelV1();
        }
        public LevelV1(ILevelMeta meta, ILevelTrack track, ILevelGameplay gameplay, IGameLevel game)
        {
            Meta = meta;
            Track = track;
            Gameplay = gameplay;
            Game = game;
        }
    }
}