using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class Prefab : IObjectScope
    {
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