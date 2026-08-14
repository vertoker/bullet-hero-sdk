using System;
using BH.SDK.Models.Enums.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector3 : IModel<IVector3>
    {
        public VectorType GetModelType();
    }
}