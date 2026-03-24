using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    public class Checkpoint : IFrame
    {
        [JsonProperty(ModelNames.Frame)]
        public int Frame { get; set; }
        
        [JsonProperty(ModelNames.Name)]
        public string Name { get; set; }
        
        [JsonProperty(ModelNames.Active)]
        public bool Active { get; set; }
        
        [JsonProperty(ModelNames.Color)]
        public IColor Color { get; set; }

        public Checkpoint()
        {
            Name = string.Empty;
            Active = true;
            Frame = 0;
            Color = new ColorValue(UnityEngine.Color.white);
        }
        public Checkpoint(string name, bool active, int frame, IColor color)
        {
            Name = name;
            Active = active;
            Frame = frame;
            Color = color;
        }
    }
}