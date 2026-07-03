using BH.SDK.Models.Enum;

namespace BH.SDK.Models.Interfaces
{
    public interface IKeyframe : IFrame
    {
        public EaseType Ease { get; set; }
    }
}