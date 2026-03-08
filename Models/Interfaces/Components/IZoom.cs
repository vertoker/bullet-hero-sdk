using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Components;

namespace BHSDK.Models.Interfaces.Components
{
    public interface IZoom : IModelReflection<ZoomV1, IZoom>
    {
        public int Frame { get; set; }
        public IFloatValue Size { get; set; }
        public EasingType Easing { get; set; }
    }
}