using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class BoolKey : IFrame
    {
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Bool)]
        public bool Value { get; set; }

        public BoolKey()
        {
            Frame = 0;
            Value = false;
        }
        public BoolKey(int frame, bool value)
        {
            Frame = frame;
            Value = value;
        }
    }
}