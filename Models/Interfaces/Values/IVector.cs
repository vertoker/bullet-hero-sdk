using BHSDK.Models.Enum.Values;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVector
    {
        public VectorType Type { get; }
        public Vector2 Get();
    }
}