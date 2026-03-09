using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Components;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Components
{
    public interface IClr : IModelReflection<ClrV1, IClr>
    {
        public int Frame { get; set; }
        public IColor Color { get; set; }
        public EaseType Ease { get; set; }
    }
}