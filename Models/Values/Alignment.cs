using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class Alignment
    {
        [JsonProperty(ModelNames.Value)]
        public IVector2 Value { get; set; }

        public static readonly Vector2Value TopLeftValue =      new(0.0f, 1.0f);
        public static readonly Vector2Value TopCenterValue =    new(0.5f, 1.0f);
        public static readonly Vector2Value TopRightValue =     new(1.0f, 1.0f);
        public static readonly Vector2Value MiddleLeftValue =   new(0.0f, 0.5f);
        public static readonly Vector2Value MiddleCenterValue = new(0.5f, 0.5f);
        public static readonly Vector2Value MiddleRightValue =  new(1.0f, 0.5f);
        public static readonly Vector2Value BottomLeftValue =   new(0.0f, 0.0f);
        public static readonly Vector2Value BottomCenterValue = new(0.5f, 0.0f);
        public static readonly Vector2Value BottomRightValue =  new(1.0f, 0.0f);
        
        // cos(-30°) / 2f - 137 / 2048 = 0.433012701892219323 - 0.06689453125 = 0.36611817064
        public static readonly Vector2Value Equilateral3Value = new(0.5f, 0.36611817064f);
        // cos(18°) / 2f - 50 / 2048 = 0.475528258147576786 - 0.0244140625 = 0.45111419564
        public static readonly Vector2Value Equilateral5Value = new(0.5f, 0.45111419564f);
        // cos((90-360/7*2)°) / 2f - 25 / 2048 = 0.487463956090911803 - 0.01220703125 = 0.47525692484
        public static readonly Vector2Value Equilateral7Value = new(0.5f, 0.47525692484f);
        // cos((90-360/9*2)°) / 2f - 15 / 2048 = 0.492403876506104029 - 0.00732421875 = 0.48507965775
        public static readonly Vector2Value Equilateral9Value = new(0.5f, 0.48507965775f);
        
        public static readonly Alignment TopLeft = new(TopLeftValue);
        public static readonly Alignment TopCenter = new(TopCenterValue);
        public static readonly Alignment TopRight = new(TopRightValue);
        public static readonly Alignment MiddleLeft = new(MiddleLeftValue);
        public static readonly Alignment MiddleCenter = new(MiddleCenterValue);
        public static readonly Alignment MiddleRight = new(MiddleRightValue);
        public static readonly Alignment BottomLeft = new(BottomLeftValue);
        public static readonly Alignment BottomCenter = new(BottomCenterValue);
        public static readonly Alignment BottomRight = new(BottomRightValue);
        
        public static readonly Alignment Equilateral3 = new(Equilateral3Value);
        public static readonly Alignment Equilateral5 = new(Equilateral5Value);
        public static readonly Alignment Equilateral7 = new(Equilateral7Value);
        public static readonly Alignment Equilateral9 = new(Equilateral9Value);
        
        public Alignment()
        {
            Value = MiddleCenterValue;
        }
        public Alignment(IVector2 value)
        {
            Value = value;
        }
    }
}