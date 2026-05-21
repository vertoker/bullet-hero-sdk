using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector2 : ICopyable<IVector2>, IEquatable<IVector2>
    {
        public VectorType GetModelType();
    }
}