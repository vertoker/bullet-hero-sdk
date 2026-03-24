using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Bln : IFrame
    {
        [JsonProperty(ModelNames.Frame)]
        public int Frame { get; set; }
        
        [JsonProperty(ModelNames.Boolean)]
        public bool Boolean { get; set; }

        public Bln()
        {
            Frame = 0;
            Boolean = false;
        }
        public Bln(int frame, bool boolean)
        {
            Frame = frame;
            Boolean = boolean;
        }
    }
}