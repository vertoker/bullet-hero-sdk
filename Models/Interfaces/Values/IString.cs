using System;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IString : ICopyable<IString>, IEquatable<IString>
    {
        public StringType GetModelType();
    }
}