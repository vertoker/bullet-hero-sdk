using System;
using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IColor : ICopyable<IColor>, IEquatable<IColor>
    {
        public ColorType GetModelType();
    }
}