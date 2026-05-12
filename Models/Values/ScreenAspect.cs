using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ScreenAspect : ICopyable<ScreenAspect>
    {
        [RuleMin(1)]
        [JsonProperty(Names.WidthShort)]
        public int Width { get; set; }
        
        [RuleMin(1)]
        [JsonProperty(Names.HeightShort)]
        public int Height { get; set; }
        
        // TODO add vertical/horizontal metadata (for phones and special modes)
        
        public float GetAspect() => Width / (float)Height;

        public ScreenAspect()
        {
            Width = 16;
            Height = 9;
        }
        public ScreenAspect(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public object Clone() => Copy();
        public ScreenAspect Copy() => new(Width, Height);
    }
}