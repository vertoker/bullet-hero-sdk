using System;
using BH.SDK.Models.Enums.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector4 : IModel<IVector4>
    {
        public VectorType GetModelType();
    }
}