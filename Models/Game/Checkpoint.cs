using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class Checkpoint
    {
        [JsonProperty("n")]
        public string Name { get; set; }
        
        [JsonProperty("a")]
        public bool Active { get; set; }
        
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("c")]
        public IColor Color { get; set; }

        public Checkpoint()
        {
            Name = string.Empty;
            Active = true;
            Frame = 0;
            Color = new ColorValue(1f, 1f, 1f, 1f);
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