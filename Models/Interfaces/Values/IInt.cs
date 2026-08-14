using System;
using BH.SDK.Models.Enums.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IInt : IModel<IInt>
    {
        public IntType GetModelType();
    }
}