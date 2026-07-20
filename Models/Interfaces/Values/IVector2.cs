using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector2 : IModel<IVector2>
    {
        public VectorType GetModelType();
    }
}