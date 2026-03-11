using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Instances;

namespace BHSDK.Models.V1.Instances
{
    public class SpriteInstanceV1 : ISpriteInstance
    {
        public int InstanceId { get; set; }
        public int ParentInstanceId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public List<IPos> Pos { get; set; }
        public List<IRot> Rot { get; set; }
        public List<ISca> Sca { get; set; }
        public List<IClr> Clr { get; set; }
        
        public bool HasCollider { get; set; }
        public int SpriteIndex { get; set; }
        public int SpriteLayer { get; set; }
        public int SublingIndex { get; set; }
        
        public SpriteInstanceV1()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            Name = string.Empty;
            IsVisible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<IPos>();
            Rot = new List<IRot>();
            Sca = new List<ISca>();
            Clr = new List<IClr>();
            
            HasCollider = true;
            SpriteIndex = 0;
            SpriteLayer = 0;
            SublingIndex = 0;
        }
        public SpriteInstanceV1(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<IPos> pos, List<IRot> rot, List<ISca> sca, List<IClr> clr, 
            bool hasCollider, int spriteIndex, int spriteLayer, int sublingIndex)
        {
            InstanceId = instanceId;
            ParentInstanceId = parentInstanceId;
            Name = name;
            IsVisible = isVisible;
            
            StartFrame = startFrame;
            EndFrame = endFrame;
            Pos = pos;
            Rot = rot;
            Sca = sca;
            Clr = clr;
            
            HasCollider = hasCollider;
            SpriteIndex = spriteIndex;
            SpriteLayer = spriteLayer;
            SublingIndex = sublingIndex;
        }
    }
}