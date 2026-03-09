using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Instances;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface IPrefabInstance : IModelReflection<PrefabInstanceV1, IPrefabInstance>
    {
        public IPrefab SourcePrefab { get; set; }
        public List<IModification> Modifications { get; set; }
    }
}