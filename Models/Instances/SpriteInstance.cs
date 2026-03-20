using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Interfaces.Instances;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class SpriteInstance : IInstance
    {
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
        public List<PosModel> Pos { get; set; }
        
        [JsonProperty("rot")]
        public List<RotModel> Rot { get; set; }
        
        [JsonProperty("sca")]
        public List<ScaModel> Sca { get; set; }
        
        [JsonProperty("clr")]
        public List<ClrModel> Clr { get; set; }
        
        
        [JsonProperty("c")]
        public bool HasCollider { get; set; }
        
        [JsonProperty("si")]
        public int SpriteIndex { get; set; }
        
        [JsonProperty("sl")]
        public int SpriteLayer { get; set; }
        
        [JsonProperty("sbi")]
        public int SublingIndex { get; set; }
        
        public SpriteInstance()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            Name = string.Empty;
            IsVisible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<PosModel>();
            Rot = new List<RotModel>();
            Sca = new List<ScaModel>();
            Clr = new List<ClrModel>();
            
            HasCollider = true;
            SpriteIndex = 0;
            SpriteLayer = 0;
            SublingIndex = 0;
        }
        public SpriteInstance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<PosModel> pos, List<RotModel> rot, List<ScaModel> sca, List<ClrModel> clr, 
            bool hasCollider, int spriteIndex, int spriteLayer, int sublingIndex)
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
            Clr = clr;
            
            HasCollider = hasCollider;
            SpriteIndex = spriteIndex;
            SpriteLayer = spriteLayer;
            SublingIndex = sublingIndex;
        }
    }
}