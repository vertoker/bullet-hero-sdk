using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IInt : ICopyable<IInt>, IEquatable<IInt>
    {
        public IntType GetModelType();
    }
}