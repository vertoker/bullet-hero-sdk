using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Game
{
    public interface ILevelObject : IModelReflection<LevelObjectV1, ILevelObject>
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
    }
}