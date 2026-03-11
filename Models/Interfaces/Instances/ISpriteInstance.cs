using BHSDK.Interfaces;
using BHSDK.Models.V1.Instances;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface ISpriteInstance : IInstance, IModelReflection<SpriteInstanceV1, ISpriteInstance>
    {
        public bool HasCollider { get; set; }
        public int SpriteIndex { get; set; }
        public int SpriteLayer { get; set; }
        public int SublingIndex { get; set; }
    }
}