using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IColorMinMax : IColor, IModelReflection<ColorMinMaxV1, IColorMinMax>
    {
        public float MinR { get; set; }
        public float MinG { get; set; }
        public float MinB { get; set; }
        public float MinA { get; set; }
        public float MaxR { get; set; }
        public float MaxG { get; set; }
        public float MaxB { get; set; }
        public float MaxA { get; set; }
    }
}