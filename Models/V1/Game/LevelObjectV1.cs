using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Game;

namespace BHSDK.Models.V1.Game
{
    public class LevelObjectV1 : ILevelObject
    {
        public int ObjectId { get; set; }
        public int ParentObjectId { get; set; }
        public bool IsActive { get; set; }
        public bool HasCollider { get; set; }
        public int SpriteIndex { get; set; }
        public int VisibleLayer { get; set; }
        
        public List<IPos> Pos { get; set; }
        public List<IRot> Rot { get; set; }
        public List<ISca> Sca { get; set; }
        public List<IClr> Clr { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        
        public int HeightIndex { get; set; }

        public LevelObjectV1()
        {
            ObjectId = 0;
            ParentObjectId = 0;
            IsActive = true;
            HasCollider = true;
            SpriteIndex = 0;
            VisibleLayer = 0;
            
            Pos = new List<IPos>();
            Rot = new List<IRot>();
            Sca = new List<ISca>();
            Clr = new List<IClr>();
            StartFrame = 0;
            EndFrame = 0;
            
            HeightIndex = 0;
        }
        public LevelObjectV1(int objectId, int parentObjectId, bool isActive, bool hasCollider, int spriteIndex, int visibleLayer, 
            List<IPos> pos, List<IRot> rot, List<ISca> sca, List<IClr> clr, int startFrame, int endFrame, int heightIndex)
        {
            ObjectId = objectId;
            ParentObjectId = parentObjectId;
            IsActive = isActive;
            HasCollider = hasCollider;
            SpriteIndex = spriteIndex;
            VisibleLayer = visibleLayer;
            Pos = pos;
            Rot = rot;
            Sca = sca;
            Clr = clr;
            StartFrame = startFrame;
            EndFrame = endFrame;
            HeightIndex = heightIndex;
        }
    }
}