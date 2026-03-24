using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class TextureObject : Object
    {
        public override ObjectType GetModelType() => ObjectType.Texture;
        
        [JsonProperty(ModelNames.Collider)]
        public bool Collider { get; set; }
        
        [JsonProperty(ModelNames.Color)]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty(ModelNames.Texture + ModelNames.Index)]
        public int TextureIndex { get; set; }
        
        [JsonProperty(ModelNames.Subling + ModelNames.Index)]
        public int SublingIndex { get; set; }
        
        public TextureObject()
        {
            Collider = true;
            Clr = new List<Clr>();
            TextureIndex = 0;
            SublingIndex = 0;
        }
        public TextureObject(int objectId, int parentObjectId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot,
            bool collider, List<Clr> clr, int textureIndex, int sublingIndex)
            : base(objectId, parentObjectId, name, isVisible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Collider = collider;
            Clr = clr;
            TextureIndex = textureIndex;
            SublingIndex = sublingIndex;
        }
    }
}