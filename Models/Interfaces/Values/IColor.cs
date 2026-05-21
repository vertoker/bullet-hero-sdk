using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IColor : ICopyable<IColor>, IEquatable<IColor>
    {
        public ColorType GetModelType();
    }
}