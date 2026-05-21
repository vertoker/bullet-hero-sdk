using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector4 : ICopyable<IVector4>, IEquatable<IVector4>
    {
        public VectorType GetModelType();
    }
}