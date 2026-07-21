using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IColor4 : IModel<IColor4>
    {
        public ColorType GetModelType();
    }
}