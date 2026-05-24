using BH.SDK.Models.Enum.Settings;

namespace BH.SDK.Models.Interfaces
{
    public interface IFrameable
    {
        public FramerateTarget FramerateTarget { get; set; }
        
        public int FixedFramerate { get; set; }
    }
}