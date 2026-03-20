using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class SpriteInstance : Instance
    {
        [JsonProperty("c")]
        public bool HasCollider { get; set; }
        
        [JsonProperty("clr")]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty("si")]
        public int SpriteIndex { get; set; }
        
        [JsonProperty("sl")]
        public int SpriteLayer { get; set; }
        
        [JsonProperty("sbi")]
        public int SublingIndex { get; set; }
        
        public SpriteInstance()
        {
            HasCollider = true;
            Clr = new List<Clr>();
            SpriteIndex = 0;
            SpriteLayer = 0;
            SublingIndex = 0;
        }
        public SpriteInstance(int instanceId, int parentInstanceId, string name, bool isVisible, int startFrame, 
            int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, List<Clr> clr, 
            bool hasCollider, int spriteIndex, int spriteLayer, int sublingIndex)
            : base(instanceId, parentInstanceId, name, isVisible, startFrame, endFrame, pos, rot, sca)
        {
            HasCollider = hasCollider;
            Clr = clr;
            SpriteIndex = spriteIndex;
            SpriteLayer = spriteLayer;
            SublingIndex = sublingIndex;
        }
    }
}