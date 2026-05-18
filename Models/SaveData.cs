using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class SaveData<TData> : ISaveData where TData : IData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.Value)]
        public TData Value { get; set; }

        public IData GetData() => Value;
        public void SetData(object data) => Value = (TData)data;

        public SaveData()
        {
            
        }
        public SaveData(TData value)
        {
            Version = value.GetVersion();
            Value = value;
        }
        public SaveData(TData value, Version version)
        {
            Version = version;
            Value = value;
        }
    }
}