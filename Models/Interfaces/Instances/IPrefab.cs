using BHSDK.Interfaces;
using BHSDK.Models.V1.Instances;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface IPrefab : IInstancesProvider, IModelReflection<PrefabV1, IPrefab>
    {
        
    }
}