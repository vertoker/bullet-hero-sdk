using System;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloat : ICopyable<IFloat>, IEquatable<IFloat>
    {
        public FloatType GetModelType();
    }
}