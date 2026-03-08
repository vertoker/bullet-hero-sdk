using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Components
{
    public interface IRot : IModelReflection<RotV1, IRot>
    {
        public int Frame { get; set; }
        public IFloatValue Angle { get; set; }
        public EasingType Easing { get; set; }
    }
}