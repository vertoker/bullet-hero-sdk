using System.Collections.Generic;
using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.V1.Game;

namespace BHSDK.Models.Interfaces.Game
{
    public interface ICameraData : IModelReflection<CameraDataV1, ICameraData>
    {
        public List<IPos> Pos { get; set; }
        public List<IRot> Rot { get; set; }
        public List<IZoom> Zoom { get; set; }
    }
}