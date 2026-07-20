using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IInt : IModel<IInt>
    {
        public IntType GetModelType();
    }
}