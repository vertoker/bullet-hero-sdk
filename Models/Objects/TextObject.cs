using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Enum.Text;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
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
    public class TextObject : RectObject, ICopyable<TextObject>, IEquatable<TextObject>, IUpdatable<TextObject>
    {
        public override ObjectType GetModelType() => ObjectType.TextObject;
        
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.Text)]
        public IString Text { get; set; }
        
        // positive with 0 - game-defined (0 is NotoSans), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.FontResourceId)]
        public int FontResourceId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(Color4Key.Frame))]
        [RuleCollectionUnique(nameof(Color4Key.Frame))]
        [JsonProperty(Names.Color)]
        public List<Color4Key> Colors { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionSorted(nameof(FloatKey.Frame))]
        [RuleCollectionUnique(nameof(FloatKey.Frame))]
        [JsonProperty(Names.FontSize)]
        public List<FloatKey> FontSizes { get; set; }
        
        [JsonProperty(Names.WordWrap)]
        public bool WordWrap { get; set; }
        
        [JsonProperty(Names.HorizontalAlignment)]
        public TextObjectHorizontalAlignment HorizontalAlignment { get; set; }
        
        [JsonProperty(Names.VerticalAlignment)]
        public TextObjectVerticalAlignment VerticalAlignment { get; set; }
        
        public TextObject()
        {
            Text = new StringValue();
            FontResourceId = 0;
            Colors = new List<Color4Key>();
            FontSizes = new List<FloatKey>();
            
            WordWrap = TextRules.WordWrap_Default;
            HorizontalAlignment = TextRules.HorizontalAlignment_Default;
            VerticalAlignment = TextRules.VerticalAlignment_Default;
        }
        public TextObject(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame, int layer,
            List<PosKey> positions, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            IString text, int fontResourceId, List<Color4Key> colors, List<FloatKey> fontSizes, bool wordWrap,
            TextObjectHorizontalAlignment horizontalAlignment, TextObjectVerticalAlignment verticalAlignment)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, layer,
                positions, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            Text = text;
            FontResourceId = fontResourceId;
            Colors = colors;
            FontSizes = fontSizes;
            WordWrap = wordWrap;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
        }
        
        public override object Clone() => CopyImpl();
        public override RectObject Copy() => CopyImpl();
        TextObject ICopyable<TextObject>.Copy() => CopyImpl();
        
        private TextObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame, Layer,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(), Text.Copy(), FontResourceId,
            Colors.CopyList(), FontSizes.CopyList(), WordWrap, HorizontalAlignment, VerticalAlignment);
        
        public void Update(TextObject src)
        {
            base.Update(src);
            
            Text = src.Text.Copy();
            FontResourceId = src.FontResourceId;
            Colors = src.Colors.CopyList();
            FontSizes = src.FontSizes.CopyList();
            
            WordWrap = src.WordWrap;
            HorizontalAlignment = src.HorizontalAlignment;
            VerticalAlignment = src.VerticalAlignment;
        }

        public override bool Equals(object obj) => obj is TextObject value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(Text);
            hashCode.Add(FontResourceId);
            hashCode.Add(Colors.GetListHashCode());
            hashCode.Add(FontSizes.GetListHashCode());
            hashCode.Add(WordWrap);
            hashCode.Add((int)HorizontalAlignment);
            hashCode.Add((int)VerticalAlignment);
            return hashCode.ToHashCode();
        }

        public bool Equals(TextObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other)
                         && EqualsTextObject(other);
            return result;
        }
        public override bool Equals(RectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case TextObject textObject:
                {
                    var result = EqualsObject(textObject)
                                 && EqualsTextObject(textObject);
                    return result;
                }
                default:
                {
                    var result = EqualsObject(other);
                    return result;
                }
            }
        }
        
        private bool EqualsTextObject(TextObject other)
        {
            var result = Text.Equals(other.Text)
                         && FontResourceId.Equals(other.FontResourceId)
                         && Colors.ListEquals(other.Colors)
                         && FontSizes.ListEquals(other.FontSizes)
                         && WordWrap == other.WordWrap
                         && HorizontalAlignment == other.HorizontalAlignment
                         && VerticalAlignment == other.VerticalAlignment;
            return result;
        }
    }
}