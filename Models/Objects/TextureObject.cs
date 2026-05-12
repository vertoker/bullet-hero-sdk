using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    [RuleContainer]
    public class TextureObject : Object, IUpdatable<TextureObject>
    {
        public override ObjectType GetModelType() => ObjectType.Texture;
        
        [JsonProperty(Names.ColliderShort)]
        public bool Collider { get; set; }
        
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectColors)]
        [RuleCollectionSorted(nameof(Clr.Frame))]
        [RuleCollectionUnique(nameof(Clr.Frame))]
        [JsonProperty(Names.Color)]
        public List<Clr> Colors { get; set; }
        
        // positive with 0 - game-defined (0 is white square), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        public TextureObject()
        {
            Collider = true;
            ColliderId = 0;
            Colors = new List<Clr>();
            TextureResourceId = 0;
        }
        public TextureObject(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> positions, List<Rot> rotations, List<Sca> scales, int layer, Alignment pivot,
            bool collider, int colliderId, List<Clr> colors, int textureResourceId)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, positions, rotations, scales, layer, pivot)
        {
            Collider = collider;
            ColliderId = colliderId;
            Colors = colors;
            TextureResourceId = textureResourceId;
        }

        public void Update(TextureObject src)
        {
            base.Update(src);
            
            Collider = src.Collider;
            ColliderId = src.ColliderId;
            Colors = src.Colors.Select(clr => clr.Copy()).ToList();
            TextureResourceId = src.TextureResourceId;
        }
    }
}