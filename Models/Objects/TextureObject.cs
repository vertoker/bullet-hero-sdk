using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class TextureObject : Object, IUpdatable<TextureObject>
    {
        public override ObjectType GetModelType() => ObjectType.Texture;
        
        [JsonProperty(Names.ColliderShort)]
        public bool Collider { get; set; }
        
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [JsonProperty(Names.Color)]
        public List<Clr> Clr { get; set; }
        
        // positive with 0 - game-defined (0 is white square), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        [JsonProperty(Names.SublingIndex)]
        public int SublingIndex { get; set; }
        
        public TextureObject()
        {
            Collider = true;
            ColliderId = 0;
            Clr = new List<Clr>();
            TextureResourceId = 0;
            SublingIndex = 0;
        }
        public TextureObject(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot,
            bool collider, int colliderId, List<Clr> clr, int textureResourceId, int sublingIndex)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Collider = collider;
            ColliderId = colliderId;
            Clr = clr;
            TextureResourceId = textureResourceId;
            SublingIndex = sublingIndex;
        }

        public void Update(TextureObject src)
        {
            base.Update(src);
            
            Collider = src.Collider;
            ColliderId = src.ColliderId;
            Clr = src.Clr.Select(clr => clr.Copy()).ToList();
            TextureResourceId = src.TextureResourceId;
            SublingIndex = src.SublingIndex;
        }
    }
}