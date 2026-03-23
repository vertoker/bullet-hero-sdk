using BHSDK.Models.Enum.Values;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVector4
    {
        public VectorType Type { get; }
        public Vector4 Get();
    }
}