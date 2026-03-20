using System.Collections.Generic;
using BHSDK.Models.Components;

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
        public List<PosModel> Pos { get; set; }
        public List<RotModel> Rot { get; set; }
        public List<ScaModel> Sca { get; set; }
        public List<ClrModel> Clr { get; set; }
    }
}