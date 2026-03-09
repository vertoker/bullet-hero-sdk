using System.Collections.Generic;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface IInstancesProvider
    {
        public List<IInstance> Instances { get; set; }
        public List<IPrefabInstance> PrefabInstances { get; set; }
    }
}