using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Base
{
    public class Instance
    {
        public virtual InstanceType GetModelType() => InstanceType.Base;
        
        [JsonProperty("id")]
        public int InstanceId { get; set; }
        
        [JsonProperty("pid")]
        public int ParentInstanceId { get; set; }
        
        [JsonProperty("n")]
        public string Name { get; set; }
        
        [JsonProperty("v")]
        public bool IsVisible { get; set; }
        
        
        [JsonProperty("sf")]
        public int StartFrame { get; set; }
        
        [JsonProperty("ef")]
        public int EndFrame { get; set; }
        
        [JsonProperty("pos")]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty("rot")]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty("sca")]
        public List<Sca> Sca { get; set; }
        
        [JsonProperty("l")]
        public int Layer { get; set; }
        
        [JsonProperty("p")]
        public Anchor Pivot { get; set; }

        public Instance()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            Name = string.Empty;
            IsVisible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<Pos>();
            Rot = new List<Rot>();
            Sca = new List<Sca>();
            Layer = 0;
            Pivot = Anchor.Center_Middle;
        }
        public Instance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot)
        {
            InstanceId = instanceId;
            ParentInstanceId = parentInstanceId;
            Name = name;
            IsVisible = isVisible;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Pos = pos;
            Rot = rot;
            Sca = sca;
            Layer = layer;
            Pivot = pivot;
        }
    }
}