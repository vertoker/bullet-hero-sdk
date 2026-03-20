using System.Collections.Generic;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class CameraData
    {
        [JsonProperty("pos")]
        public List<PosModel> Pos { get; set; }
        
        [JsonProperty("rot")]
        public List<RotModel> Rot { get; set; }
        
        [JsonProperty("zoom")]
        public List<ZoomModel> Zoom { get; set; }
        
        [JsonProperty("clr")]
        public List<ClrModel> Clr { get; set; }

        public CameraData()
        {
            Pos = new List<PosModel>();
            Rot = new List<RotModel>();
            Zoom = new List<ZoomModel>();
            Clr = new List<ClrModel>();
        }
        public CameraData(List<PosModel> pos, List<RotModel> rot, List<ZoomModel> zoom, List<ClrModel> clr)
        {
            Pos = pos;
            Rot = rot;
            Zoom = zoom;
            Clr = clr;
        }
    }
}