using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IVector3 : IModel<IVector3>
    {
        public VectorType GetModelType();
    }
}