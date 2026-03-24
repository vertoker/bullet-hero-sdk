using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class ScreenAspect
    {
        [JsonProperty(ModelNames.Width)]
        public int Width { get; set; }
        
        [JsonProperty(ModelNames.Height)]
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
    }
}