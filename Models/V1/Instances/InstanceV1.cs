using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Instances;

namespace BHSDK.Models.V1.Instances
{
    public class InstanceV1 : IInstance
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

        public InstanceV1()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            IsActive = true;
            HasCollider = true;
            SpriteIndex = 0;
            SpriteLayer = 0;
            SublingIndex = 0;
            
            Pos = new List<IPos>();
            Rot = new List<IRot>();
            Sca = new List<ISca>();
            Clr = new List<IClr>();
            StartFrame = 0;
            EndFrame = 0;
        }
        public InstanceV1(int instanceId, int parentInstanceId, bool isActive, bool hasCollider, 
            int spriteIndex, int spriteLayer, int sublingIndex, 
            List<IPos> pos, List<IRot> rot, List<ISca> sca, List<IClr> clr, int startFrame, int endFrame)
        {
            InstanceId = instanceId;
            ParentInstanceId = parentInstanceId;
            IsActive = isActive;
            HasCollider = hasCollider;
            SpriteIndex = spriteIndex;
            SpriteLayer = spriteLayer;
            SublingIndex = sublingIndex;
            
            Pos = pos;
            Rot = rot;
            Sca = sca;
            Clr = clr;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }
    }
}