using System.Collections.Generic;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class CameraEvents
    {
        [JsonProperty("pos")]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty("rot")]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty("zm")]
        public List<Zoom> Zoom { get; set; }
        
        [JsonProperty("shk")]
        public List<Shake> Shake { get; set; }

        public CameraEvents()
        {
            Pos = new List<Pos>();
            Rot = new List<Rot>();
            Zoom = new List<Zoom>();
            Shake = new List<Shake>();
        }
        public CameraEvents(List<Pos> pos, List<Rot> rot, 
            List<Zoom> zoom, List<Shake> shake)
        {
            Pos = pos;
            Rot = rot;
            Zoom = zoom;
            Shake = shake;
        }
    }
}