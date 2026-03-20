using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BHSDK.Models.Values
{
    public class VectorCircle : IVector
    {
        [JsonProperty("x")]
        public float X { get; set; }
        
        [JsonProperty("y")]
        public float Y { get; set; }
        
        [JsonProperty("r")]
        public float Radius { get; set; }
        
        public VectorCircle()
        {
            X = 0f;
            Y = 0f;
            Radius = 1f;
        }
        public VectorCircle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }
        public VectorCircle(IFloat x, IFloat y, IFloat radius)
        {
            X = x.Get();
            Y = y.Get();
            Radius = radius.Get();
        }

        public VectorType Type => VectorType.RandomCircle;
        public Vector2 Get()
        {
            // https://www.youtube.com/watch?v=4y_nmpv-9lI
            // Use distribution through Sqrt, because it uses one number.
            // Not optimal, but random predicted
            var distance = Mathf.Sqrt(Random.value);
            var angle = Random.Range(0f, 2f * Mathf.PI);

            var x = distance * Mathf.Cos(angle) * Radius + X;
            var y = distance * Mathf.Sin(angle) * Radius + Y;
            return new Vector2(x, y);
        }
    }
}