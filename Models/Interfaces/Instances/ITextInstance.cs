using BHSDK.Interfaces;
using BHSDK.Models.Enum;
using BHSDK.Models.V1.Instances;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface ITextInstance : IInstance, IModelReflection<TextInstanceV1, ITextInstance>
    {
        public string Text { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public Anchor Alignment { get; set; }
    }
}