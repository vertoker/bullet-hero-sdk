using System;

namespace BH.SDK.Models.Interfaces.SaveData
{
    public interface ISaveData
    {
        public Version Version { get; set; }

        public IData GetData();
        public void SetData(object data);
    }
}