using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class Vector : IVector
    {
        public float X { get; set; }
        public float Y { get; set; }
        
        public Vector2 GetRandom() => new(X, Y);
    }
}