using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    public class Marker : IFrame
    {
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [JsonProperty(Names.Description)]
        public string Description { get; set; }
        
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }

        public Marker()
        {
            Name = string.Empty;
            Description = string.Empty;
            Frame = 0;
            Color = new ColorValue();
        }
        public Marker(string name, string description, int frame, IColor color)
        {
            Name = name;
            Description = description;
            Frame = frame;
            Color = color;
        }
    }
}