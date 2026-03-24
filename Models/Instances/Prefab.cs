using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class Prefab : IInstancesProvider
    {
        [JsonProperty(ModelNames.Instance)]
        public List<Instance> Instances { get; set; }
        
        [JsonProperty(ModelNames.ParentInstance)]
        public List<PrefabInstance> PrefabInstances { get; set; }

        public Prefab()
        {
            Instances = new List<Instance>();
            PrefabInstances = new List<PrefabInstance>();
        }
        public Prefab(List<Instance> instances, List<PrefabInstance> prefabInstances)
        {
            Instances = instances;
            PrefabInstances = prefabInstances;
        }
    }
}