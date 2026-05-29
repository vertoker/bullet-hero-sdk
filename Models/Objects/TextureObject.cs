using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class TextureObject : RectObject, ICopyable<TextureObject>, IEquatable<TextureObject>, IUpdatable<TextureObject>
    {
        public override ObjectType GetModelType() => ObjectType.Texture;
        
        [JsonProperty(Names.ColliderShort)]
        public bool Collider { get; set; }
        
        [RuleMax(IdRules.MaxColliderId)]
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(ColorKey.Frame))]
        [RuleCollectionUnique(nameof(ColorKey.Frame))]
        [JsonProperty(Names.Color)]
        public List<ColorKey> Colors { get; set; }
        
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        public TextureObject()
        {
            Collider = true;
            ColliderId = 0;
            Colors = new List<ColorKey>();
            TextureResourceId = 0;
        }

        public TextureObject(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame,
            List<PosKey> positions, List<LayerKey> layers, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            bool collider, int colliderId, List<ColorKey> colors, int textureResourceId)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame,
                positions, layers, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
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
            Positions.CopyList(), Layers.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(),
            Collider, ColliderId, Colors.CopyList(), TextureResourceId);

        public void Update(TextureObject src)
        {
            base.Update(src);
            
            Collider = src.Collider;
            ColliderId = src.ColliderId;
            Colors = src.Colors.CopyList();
            TextureResourceId = src.TextureResourceId;
        }

        public override bool Equals(object obj) => obj is TextureObject value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Collider, ColliderId, Colors.GetListHashCode(), TextureResourceId);

        public bool Equals(TextureObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other)
                         && EqualsRectObject(other)
                         && EqualsTextureObject(other);
            return result;
        }
        public override bool Equals(RectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case TextureObject textureObject:
                {
                    var result = EqualsObject(textureObject)
                                 && EqualsRectObject(textureObject)
                                 && EqualsTextureObject(textureObject);
                    return result;
                }
                default:
                {
                    var result = EqualsObject(other)
                                 && EqualsRectObject(other);
                    return result;
                }
            }
        }
        public override bool Equals(Object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case TextureObject textureObject:
                {
                    var result = EqualsObject(textureObject)
                                 && EqualsRectObject(textureObject)
                                 && EqualsTextureObject(textureObject);
                    return result;
                }
                case RectObject rectObject:
                {
                    var result = EqualsObject(rectObject)
                                 && EqualsRectObject(rectObject);
                    return result;
                }
                default:
                {
                    var result = EqualsObject(other);
                    return result;
                }
            }
        }

        private bool EqualsTextureObject(TextureObject other)
        {
            var result = Collider == other.Collider
                         && ColliderId.Equals(other.ColliderId)
                         && Colors.ListEquals(other.Colors)
                         && TextureResourceId.Equals(other.TextureResourceId);
            return result;
        }
    }
}