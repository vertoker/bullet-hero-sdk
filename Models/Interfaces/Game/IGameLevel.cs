using BHSDK.Interfaces;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Game
{
    public interface IGameLevel : IModelReflection<GameLevelV1, IGameLevel>
    {
        public int Seed { get; set; }
    }
}