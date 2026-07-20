using System;
using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IString : IModel<IString>
    {
        public StringType GetModelType();
    }
}