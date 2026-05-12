using System;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVector3 : ICopyable<IVector3>, IEquatable<IVector3>
    {
        public VectorType GetModelType();
    }
}