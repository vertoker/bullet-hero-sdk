using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class Alignment : ICopyable<Alignment>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinSca, ValueRules.MaxSca)]
        [JsonProperty(Names.ValueShort)]
        public IVector2 Value { get; set; }

        public static Vector2Value TopLeftValue =>      new(0.0f, 1.0f);
        public static Vector2Value TopCenterValue =>    new(0.5f, 1.0f);
        public static Vector2Value TopRightValue =>     new(1.0f, 1.0f);
        public static Vector2Value MiddleLeftValue =>   new(0.0f, 0.5f);
        public static Vector2Value MiddleCenterValue => new(0.5f, 0.5f);
        public static Vector2Value MiddleRightValue =>  new(1.0f, 0.5f);
        public static Vector2Value BottomLeftValue =>   new(0.0f, 0.0f);
        public static Vector2Value BottomCenterValue => new(0.5f, 0.0f);
        public static Vector2Value BottomRightValue =>  new(1.0f, 0.0f);
        
        public static Vector2Value Equilateral3Value => new(0.5f, 0.355570f); // 1f / cos(-30°) / 2f = 0.577350269189625764
        public static Vector2Value Equilateral5Value => new(0.5f, 0.449739f); // 1f / cos(18°) / 2f = 0.525731112119133606
        public static Vector2Value Equilateral7Value => new(0.5f, 0.474765f); // 1f / cos((90-360/7*2)°) / 2f = 0.512858431636276949
        public static Vector2Value Equilateral9Value => new(0.5f, 0.484907f); // 1f / cos((90-360/9*2)°) / 2f = 0.507713305942872492
        
        public static Alignment TopLeft => new(TopLeftValue);
        public static Alignment TopCenter => new(TopCenterValue);
        public static Alignment TopRight => new(TopRightValue);
        public static Alignment MiddleLeft => new(MiddleLeftValue);
        public static Alignment MiddleCenter => new(MiddleCenterValue);
        public static Alignment MiddleRight => new(MiddleRightValue);
        public static Alignment BottomLeft => new(BottomLeftValue);
        public static Alignment BottomCenter => new(BottomCenterValue);
        public static Alignment BottomRight => new(BottomRightValue);
        
        public static Alignment Equilateral3 => new(Equilateral3Value);
        public static Alignment Equilateral5 => new(Equilateral5Value);
        public static Alignment Equilateral7 => new(Equilateral7Value);
        public static Alignment Equilateral9 => new(Equilateral9Value);
        
        public Alignment()
        {
            Value = MiddleCenterValue;
        }
        public Alignment(IVector2 value)
        {
            Value = value;
        }

        public Alignment Copy() => new(Value.Copy());
    }
}