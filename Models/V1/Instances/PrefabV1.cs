using System.Collections.Generic;
using BHSDK.Models.Interfaces.Instances;

namespace BHSDK.Models.V1.Instances
{
    public class PrefabV1 : IPrefab
    {
        public List<IInstance> Instances { get; set; }
        public List<IPrefabInstance> PrefabInstances { get; set; }

        public PrefabV1()
        {
            Instances = new List<IInstance>();
            PrefabInstances = new List<IPrefabInstance>();
        }
        public PrefabV1(List<IInstance> instances, List<IPrefabInstance> prefabInstances)
        {
            Instances = instances;
            PrefabInstances = prefabInstances;
        }
    }
}