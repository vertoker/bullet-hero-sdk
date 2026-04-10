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
        
        public static readonly Alignment TopLeft = new(TopLeftValue);
        public static readonly Alignment TopCenter = new(TopCenterValue);
        public static readonly Alignment TopRight = new(TopRightValue);
        public static readonly Alignment MiddleLeft = new(MiddleLeftValue);
        public static readonly Alignment MiddleCenter = new(MiddleCenterValue);
        public static readonly Alignment MiddleRight = new(MiddleRightValue);
        public static readonly Alignment BottomLeft = new(BottomLeftValue);
        public static readonly Alignment BottomCenter = new(BottomCenterValue);
        public static readonly Alignment BottomRight = new(BottomRightValue);
        
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