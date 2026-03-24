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
        
        [JsonProperty(ModelNames.Position)]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty(ModelNames.Rotation)]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty(ModelNames.Zoom)]
        public List<Zoom> Zoom { get; set; }
        
        [JsonProperty(ModelNames.Shake)]
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