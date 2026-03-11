using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;

namespace BHSDK.Models.Interfaces.Instances
{
    public interface IInstance
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
    }
}