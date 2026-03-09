using System.Collections.Generic;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Game;

namespace BHSDK.Models.V1.Game
{
    public class CameraDataV1 : ICameraData
    {
        public List<IPos> Pos { get; set; }
        public List<IRot> Rot { get; set; }
        public List<IZoom> Zoom { get; set; }
        public List<IClr> Clr { get; set; }

        public CameraDataV1()
        {
            Pos = new List<IPos>();
            Rot = new List<IRot>();
            Zoom = new List<IZoom>();
            Clr = new List<IClr>();
        }
        public CameraDataV1(List<IPos> pos, List<IRot> rot, List<IZoom> zoom, List<IClr> clr)
        {
            Pos = pos;
            Rot = rot;
            Zoom = zoom;
            Clr = clr;
        }
    }
}