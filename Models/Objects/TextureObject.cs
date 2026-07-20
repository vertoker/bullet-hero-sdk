using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class TextureObject : RectObject, IModel<TextureObject>, IUpdatable<TextureObject>
    {
        public override ObjectType GetModelType() => ObjectType.TextureObject;
        
        [JsonProperty(Names.ColliderId)]
        public ColliderId ColliderId { get; set; }
        
        [JsonProperty(Names.TextureResourceId)]
        public TextureResourceId TextureResourceId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionUnique(nameof(IColor4X4Key.Frame))]
        [JsonProperty(Names.Color)]
        public List<IColor4X4Key> Colors { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionUnique(nameof(UVKey.Frame))]
        [JsonProperty(Names.UV)]
        public List<UVKey> UVs { get; set; }
        
        public TextureObject()
        {
            ColliderId = ColliderId.Square;
            TextureResourceId = TextureResourceId.Square;
            Colors = new List<IColor4X4Key>();
            UVs = new List<UVKey>();
        }
        public TextureObject(ObjectId objectId, ObjectId parentObjectId, string name, bool visible, int startFrame, int endFrame, int layer,
            List<PosKey> positions, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            ColliderId colliderId, TextureResourceId textureResourceId, List<IColor4X4Key> colors, List<UVKey> uvs)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, layer,
                positions, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            ColliderId = colliderId;
            TextureResourceId = textureResourceId;
            Colors = colors;
            UVs = uvs;
        }
        public override void Reset()
        {
            base.Reset();
            ColliderId = ColliderId.Square;
            TextureResourceId = TextureResourceId.Square;
            Colors.Clear();
            UVs.Clear();
        }
        
        public override object Clone() => CopyImpl();
        public override RectObject Copy() => CopyImpl();
        TextureObject ICopyable<TextureObject>.Copy() => CopyImpl();
        
        private TextureObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame, Layer,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(),
            ColliderId, TextureResourceId, Colors.CopyList(), UVs.CopyList());

        public void Update(TextureObject src)
        {
            base.Update(src);
            
            ColliderId = src.ColliderId;
            TextureResourceId = src.TextureResourceId;
            Colors = src.Colors.CopyList();
            UVs = src.UVs.CopyList();
        }

        public override bool Equals(object obj) => obj is TextureObject value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            ColliderId, TextureResourceId, Colors.GetListHashCode(), UVs.GetListHashCode());

        public bool Equals(TextureObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other)
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
                                 && EqualsTextureObject(textureObject);
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
            var result = ColliderId.Equals(other.ColliderId)
                         && TextureResourceId.Equals(other.TextureResourceId)
                         && Colors.ListEquals(other.Colors)
                         && UVs.ListEquals(other.UVs);
            return result;
        }
    }
}