using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Instances;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface IInstance : IModelReflection<InstanceV1, IInstance>
    {
        public int InstanceId { get; set; }
        public int ParentInstanceId { get; set; }
        public bool IsActive { get; set; }
        public bool HasCollider { get; set; }
        public int SpriteIndex { get; set; }
        public int SpriteLayer { get; set; }
        public int SublingIndex { get; set; }
        
        public List<IPos> Pos { get; set; }
        public List<IRot> Rot { get; set; }
        public List<ISca> Sca { get; set; }
        public List<IClr> Clr { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
    }
}