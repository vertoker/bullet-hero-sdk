using System.Collections.Generic;
using BHSDK.Models.Interfaces.Instances;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class Prefab : IInstancesProvider
    {
        [JsonProperty("is")]
        public List<IInstance> Instances { get; set; }
        
        [JsonProperty("pis")]
        public List<PrefabInstance> PrefabInstances { get; set; }

        public Prefab()
        {
            Instances = new List<IInstance>();
            PrefabInstances = new List<PrefabInstance>();
        }
        public Prefab(List<IInstance> instances, List<PrefabInstance> prefabInstances)
        {
            Instances = instances;
            PrefabInstances = prefabInstances;
        }
    }
}