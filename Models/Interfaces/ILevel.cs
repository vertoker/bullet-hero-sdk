using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1;

namespace BHSDK.Models.Interfaces
{
    public interface ILevel : IModelReflection<LevelV1, ILevel>
    {
        public ILevelMeta Meta { get; set; }
        public ILevelTrack Track { get; set; }
        public IGameLevel Game { get; set; }
    }
}