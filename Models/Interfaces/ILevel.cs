using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.V1;

namespace BHSDK.Models.Interfaces
{
    public interface ILevel : IModelReflection<LevelV1, ILevel>, ILevelCompatibilityModel
    {
        public ILevelMeta Meta { get; set; }
        public ILevelTrack Track { get; set; }
        public ILevelRules Rules { get; set; }
        public IGameLevel Game { get; set; }
    }
}