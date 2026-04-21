using System.Collections.Generic;
using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
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
        
        // positive with 0 - game-defined (0 is white square), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(ModelNames.Texture + ModelNames.Resource + ModelNames.Id)]
        public int TextureResourceId { get; set; }
        
        [JsonProperty(ModelNames.Subling + ModelNames.Index)]
        public int SublingIndex { get; set; }
        
        public TextureObject()
        {
            Collider = true;
            Clr = new List<Clr>();
            TextureResourceId = 0;
            SublingIndex = 0;
        }
        public TextureObject(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot,
            bool collider, List<Clr> clr, int textureResourceId, int sublingIndex)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Collider = collider;
            Clr = clr;
            TextureResourceId = textureResourceId;
            SublingIndex = sublingIndex;
        }
    }
}