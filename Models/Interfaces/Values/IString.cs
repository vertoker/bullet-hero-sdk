using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IString : ICopyable<IString>, IEquatable<IString>
    {
        public StringType GetModelType();
    }
}