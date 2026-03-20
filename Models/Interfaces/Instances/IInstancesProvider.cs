using System.Collections.Generic;
using BHSDK.Models.Instances;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface IInstancesProvider
    {
        public List<IInstance> Instances { get; set; }
        public List<PrefabInstance> PrefabInstances { get; set; }
    }
}