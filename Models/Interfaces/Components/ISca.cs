using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Components
{
    public interface ISca : IModelReflection<ScaV1, ISca>
    {
        public int Frame { get; set; }
        public IVector Vector { get; set; }
        public EasingType Easing { get; set; }
    }
}