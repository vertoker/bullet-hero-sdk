using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IFloat : IModel<IFloat>
    {
        public FloatType GetModelType();
    }
}