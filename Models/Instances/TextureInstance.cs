using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class TextureInstance : Instance
    {
        public override InstanceType GetModelType() => InstanceType.Texture;
        
        [JsonProperty("c")]
        public bool HasCollider { get; set; }
        
        [JsonProperty("clr")]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty("si")]
        public int SpriteIndex { get; set; }
        
        [JsonProperty("sbi")]
        public int SublingIndex { get; set; }
        
        public TextureInstance()
        {
            HasCollider = true;
            Clr = new List<Clr>();
            SpriteIndex = 0;
            SublingIndex = 0;
        }
        public TextureInstance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot,
            bool hasCollider, List<Clr> clr, int spriteIndex, int sublingIndex)
            : base(instanceId, parentInstanceId, name, isVisible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            HasCollider = hasCollider;
            Clr = clr;
            SpriteIndex = spriteIndex;
            SublingIndex = sublingIndex;
        }
    }
}