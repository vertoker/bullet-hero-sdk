using System;
using BH.SDK.Models.Interfaces.SaveData;

namespace BH.SDK.Serialization.DataConverters
{
    public interface IDataConverter
    {
        bool CanConvert(Type sourceType, Type targetType);
        TValue Convert<TValue>(IData source) where TValue : IData;
    }
}