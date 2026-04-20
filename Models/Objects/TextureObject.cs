using System.Collections.Generic;
using BHSDK.Models.Base;
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
        
        // How textureId works
        // TextureId divide to 2 fields: game-defined and user-defined
        // all game-defined starts with 0 and counts to positive (0, 1, 2, 3...)
        // all user-defined starts with 1 and counts to negative (-1, -2, -3, -4...)
        
        // Internally, they converted from textureId to textureIndex with simple function
        // textureIndex = textureId >= 0 ? textureId : <count of game-defined textures> - textureId - 1
        // Count of game-defined textures can be changed with Level version, this validates by validators
        
        [JsonProperty(ModelNames.Texture + ModelNames.Id)]
        public int TextureId { get; set; }
        
        [JsonProperty(ModelNames.Subling + ModelNames.Index)]
        public int SublingIndex { get; set; }
        
        public TextureObject()
        {
            Collider = true;
            Clr = new List<Clr>();
            TextureId = 0;
            SublingIndex = 0;
        }
        public TextureObject(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot,
            bool collider, List<Clr> clr, int textureIndex, int sublingIndex)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Collider = collider;
            Clr = clr;
            TextureId = textureIndex;
            SublingIndex = sublingIndex;
        }
    }
}