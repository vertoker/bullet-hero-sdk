using System;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IInt : ICopyable<IInt>, IEquatable<IInt>
    {
        public IntType GetModelType();
    }
}