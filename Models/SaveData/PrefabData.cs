using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class PrefabData : ISaveData
    {
        [JsonProperty(ModelNames.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(ModelNames.Prefab)]
        public IPrefab Prefab { get; set; }

        public IData GetData() => Prefab;
        public void SetData(object data) => Prefab = data as IPrefab;

        public PrefabData()
        {
            
        }
        public PrefabData(IPrefab prefab)
        {
            Version = prefab.GetVersion();
            Prefab = prefab;
        }
    }
}