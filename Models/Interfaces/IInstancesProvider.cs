using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Instances;

namespace BHSDK.Models.Interfaces
{
    public interface IInstancesProvider
    {
        public List<Instance> Instances { get; set; }
        public List<PrefabInstance> PrefabInstances { get; set; }
    }
}