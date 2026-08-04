using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Enum.Text;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    /// <summary>
    /// A rect that renders text. Purely visual - unlike TextureObject it carries no collider, so
    /// text can never kill the player, and its rect acts as the layout box rather than a hitbox.
    /// </summary>
    [RuleContainer]
    public class TextObject : RectObject, IModel<TextObject>, IUpdatable<TextObject>
    {
        public override ObjectType GetModelType() => ObjectType.TextObject;

        /// <summary> The text to show, localizable - a level can read differently per language
        /// without duplicating the object. </summary>
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.Text)]
        public IString Text { get; set; }

        // positive with 0 - game-defined (0 is NotoSans), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file

        /// <summary> Typeface, defaulting to the bundled one so a level always renders even without
        /// its own font shipped alongside. </summary>
        [RuleIPrimitiveIntNotNull]
        [JsonProperty(Names.FontResourceId)]
        public FontResourceId FontResourceId { get; set; }

        /// <summary> Tint track. Flat Color4Key only, not the four-corner family a TextureObject
        /// uses - glyphs have no quad to grade across. </summary>
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionUnique(nameof(Color4Key.Frame))]
        [JsonProperty(Names.Color)]
        public List<Color4Key> Colors { get; set; }

        /// <summary> Font size track, animated independently of the object's Scales - one resizes
        /// glyphs, the other stretches the whole rendered block. </summary>
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionUnique(nameof(FloatKey.Frame))]
        [JsonProperty(Names.FontSize)]
        public List<FloatKey> FontSizes { get; set; }

        /// <summary> Whether long lines wrap at the rect's width instead of overflowing it. </summary>
        [JsonProperty(Names.WordWrap)]
        public bool WordWrap { get; set; }

        /// <summary> Horizontal placement of the text inside its rect. </summary>
        [JsonProperty(Names.HorizontalAlignment)]
        public TextObjectHorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary> Vertical placement of the text inside its rect. </summary>
        [JsonProperty(Names.VerticalAlignment)]
        public TextObjectVerticalAlignment VerticalAlignment { get; set; }
        
        public TextObject()
        {
            Text = new StringValue();
            FontResourceId = FontResourceId.Default;
            Colors = new List<Color4Key>();
            FontSizes = new List<FloatKey>();
            
            WordWrap = TextRules.WordWrap_Default;
            HorizontalAlignment = TextRules.HorizontalAlignment_Default;
            VerticalAlignment = TextRules.VerticalAlignment_Default;
        }
        public TextObject(ObjectId objectId, ObjectId parentObjectId, string name, bool visible, int startFrame, int endFrame, int layer,
            List<PosKey> positions, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            IString text, FontResourceId fontResourceId, List<Color4Key> colors, List<FloatKey> fontSizes, bool wordWrap,
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
        public override void Reset()
        {
            base.Reset();
            Text = new StringValue();
            FontResourceId = FontResourceId.Default;
            Colors.Clear();
            FontSizes.Clear();
            
            WordWrap = TextRules.WordWrap_Default;
            HorizontalAlignment = TextRules.HorizontalAlignment_Default;
            VerticalAlignment = TextRules.VerticalAlignment_Default;
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