using System;
using BHSDK.Models.Interfaces.SaveData;

namespace BHSDK.Serialization.DataConverters
{
    public interface IDataConverter
    {
        bool CanConvert(Type sourceType, Type targetType);
        TValue Convert<TValue>(IData source) where TValue : IData;
    }
}