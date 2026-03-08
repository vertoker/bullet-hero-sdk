using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Components
{
    public interface IPos : IModelReflection<PosV1, IPos>
    {
        public int Frame { get; set; }
        public IVector Vector { get; set; }
        public Anchor Anchor { get; set; }
        public EasingType Easing { get; set; }
    }
}