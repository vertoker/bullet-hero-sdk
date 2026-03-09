using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Components;

namespace BHSDK.Models.Interfaces.Components
{
    public interface IZoom : IModelReflection<ZoomV1, IZoom>
    {
        public int Frame { get; set; }
        public IFloat Size { get; set; }
        public EaseType Ease { get; set; }
    }
}