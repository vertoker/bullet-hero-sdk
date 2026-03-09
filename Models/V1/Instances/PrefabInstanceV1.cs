using System.Collections.Generic;
using BHSDK.Models.Interfaces.Instances;
using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.V1.Instances
{
    public class PrefabInstanceV1 : IPrefabInstance
    {
        public IPrefab SourcePrefab { get; set; }
        public List<IModification> Modifications { get; set; }

        public PrefabInstanceV1()
        {
            SourcePrefab = new PrefabV1();
            Modifications = new List<IModification>();
        }
        public PrefabInstanceV1(IPrefab sourcePrefab, List<IModification> modifications)
        {
            SourcePrefab = sourcePrefab;
            Modifications = modifications;
        }
    }
}