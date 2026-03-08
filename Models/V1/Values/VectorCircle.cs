using System;
using BHSDK.Models.Interfaces.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Values
{
    public class VectorCircle : IVector
    {
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Radius { get; set; }

        public Vector2 GetRandom()
        {
            throw new NotImplementedException(); // TODO 
        }
    }
}