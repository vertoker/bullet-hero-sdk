using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector3 : ICopyable<IVector3>, IEquatable<IVector3>
    {
        public VectorType GetModelType();
    }
}