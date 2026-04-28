using BHSDK.Models.Enum.Values;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVector3 : ICopyable<IVector3>
    {
        public VectorType GetModelType();
        public Vector3 Get();
    }
}