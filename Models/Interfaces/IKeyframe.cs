using BH.SDK.Models.Enums;

namespace BH.SDK.Models.Interfaces
{
    public interface IKeyframe : IFrame
    {
        public EaseType Ease { get; set; }
    }
}