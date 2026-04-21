using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;
using Object = BHSDK.Models.Objects.Object;

namespace BHSDK.Models.Objects
{
    public class Prefab : IObjectScope, IPrefab
    {
        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(ModelNames.Object)]
        public List<Object> Objects { get; set; }
        
        [JsonProperty(ModelNames.ParentObject)]
        public List<PrefabObject> PrefabObjects { get; set; }

        public Prefab()
        {
            Objects = new List<Object>();
            PrefabObjects = new List<PrefabObject>();
        }
        public Prefab(List<Object> objects, List<PrefabObject> prefabObjects)
        {
            Objects = objects;
            PrefabObjects = prefabObjects;
        }
    }
}