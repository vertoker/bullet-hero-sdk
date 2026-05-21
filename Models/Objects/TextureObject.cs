using System;
using System.Collections.Generic;
using System.Linq;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class TextureObject : Object,
        ICopyable<TextureObject>, IEquatable<TextureObject>, IUpdatable<TextureObject>
    {
        public override ObjectType GetModelType() => ObjectType.Texture;
        
        [JsonProperty(Names.ColliderShort)]
        public bool Collider { get; set; }
        
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxObjectKeyframes)]
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
        
        public override object Clone() => CopyImpl();
        public override Object Copy() => CopyImpl();
        TextureObject ICopyable<TextureObject>.Copy() => CopyImpl();
        
        private TextureObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Layer, Pivot.Copy(),
            Collider, ColliderId, Colors.CopyList(), TextureResourceId);

        public void Update(TextureObject src)
        {
            base.Update(src);
            
            Collider = src.Collider;
            ColliderId = src.ColliderId;
            Colors = src.Colors.Select(clr => clr.Copy()).ToList();
            TextureResourceId = src.TextureResourceId;
        }

        public override bool Equals(object obj) => obj is TextureObject value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Collider, ColliderId, Colors.GetListHashCode(), TextureResourceId);

        public bool Equals(TextureObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Collider == other.Collider
                         && ColliderId.Equals(other.ColliderId)
                         && Colors.ListEquals(other.Colors)
                         && TextureResourceId.Equals(other.TextureResourceId);
            return result;
        }
    }
}