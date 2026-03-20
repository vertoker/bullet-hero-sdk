using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class Marker
    {
        [JsonProperty("n")]
        public string Name { get; set; }
        
        [JsonProperty("d")]
        public string Description { get; set; }
        
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("c")]
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