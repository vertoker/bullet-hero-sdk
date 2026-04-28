using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class ScreenAspect : ICopyable<ScreenAspect>
    {
        [JsonProperty(Names.WidthShort)]
        public int Width { get; set; }
        
        [JsonProperty(Names.HeightShort)]
        public int Height { get; set; }
        
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

        public ScreenAspect Copy() => new(Width, Height);
    }
}