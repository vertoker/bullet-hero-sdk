using System.Collections.Generic;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class CameraEvents
    {
        // Camera - is a unique instance. All new instances by default inherited from camera.
        // Camera has instanceId and this is always -1 (because 0 - is a fallback).
        // Camera GO != camera info for parenting, this info just duplicate from camera.transform 
        
        [JsonProperty("pos")]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty("rot")]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty("zm")]
        public List<Zoom> Zoom { get; set; }
        
        [JsonProperty("shk")]
        public List<Shake> Shakes { get; set; }

        public CameraEvents()
        {
            Pos = new List<Pos>();
            Rot = new List<Rot>();
            Zoom = new List<Zoom>();
            Shakes = new List<Shake>();
        }
        public CameraEvents(List<Pos> pos, List<Rot> rot, 
            List<Zoom> zoom, List<Shake> shakes)
        {
            Pos = pos;
            Rot = rot;
            Zoom = zoom;
            Shakes = shakes;
        }
    }
}