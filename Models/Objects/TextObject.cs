using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Text;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class TextObject : Object, IUpdatable<TextObject>
    {
        public override ObjectType GetModelType() => ObjectType.Text;
        
        [JsonProperty(Names.Text)]
        public IString Text { get; set; }
        
        // positive with 0 - game-defined (0 is NotoSans), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.FontResourceId)]
        public int FontResourceId { get; set; }
        
        [JsonProperty(Names.Size)]
        public List<Sca> Sizes { get; set; }
        
        [JsonProperty(Names.Color)]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty(Names.FontSize)]
        public List<FloatKey> FontSizes { get; set; }
        
        
        [JsonProperty(Names.Direction)]
        public TextObjectDirection Direction { get; set; }
        
        [JsonProperty(Names.WordWrap)]
        public bool WordWrap { get; set; }
        
        [JsonProperty(Names.HorizontalAlignment)]
        public TextObjectHorizontalAlignment HorizontalAlignment { get; set; }
        
        [JsonProperty(Names.VerticalAlignment)]
        public TextObjectVerticalAlignment VerticalAlignment { get; set; }
        
        [JsonProperty(Names.OverEdge)]
        public TextObjectOverEdge OverEdge { get; set; }
        
        [JsonProperty(Names.UnderEdge)]
        public TextObjectUnderEdge UnderEdge { get; set; }
        
        [JsonProperty(Names.Distrib)]
        public TextObjectLeadingDistribution LeadingDistribution { get; set; }
        
        public TextObject()
        {
            Text = new StringValue(string.Empty);
            FontResourceId = 0;
            Sizes = new List<Sca>();
            Clr = new List<Clr>();
            FontSizes = new List<FloatKey>();
            
            Direction = TextStatic.DirectionDefault;
            WordWrap = TextStatic.WordWrapDefault;
            HorizontalAlignment = TextStatic.HorizontalAlignmentDefault;
            VerticalAlignment = TextStatic.VerticalAlignmentDefault;
            OverEdge = TextStatic.OverEdgeDefault;
            UnderEdge = TextStatic.UnderEdgeDefault;
            LeadingDistribution = TextStatic.LeadingDistributionDefault;
        }
        public TextObject(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame, 
            List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot, IString text, int fontResourceId,
            List<Sca> sizes, List<Clr> clr, List<FloatKey> fontSizes, TextObjectDirection direction, bool wordWrap, 
            TextObjectHorizontalAlignment horizontalAlignment, TextObjectVerticalAlignment verticalAlignment, 
            TextObjectOverEdge overEdge, TextObjectUnderEdge underEdge, TextObjectLeadingDistribution leadingDistribution)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Text = text;
            FontResourceId = fontResourceId;
            Sizes = sizes;
            Clr = clr;
            FontSizes = fontSizes;
            
            Direction = direction;
            WordWrap = wordWrap;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            OverEdge = overEdge;
            UnderEdge = underEdge;
            LeadingDistribution = leadingDistribution;
        }

        public void Update(TextObject src)
        {
            base.Update(src);
            
            Text = src.Text;
            FontResourceId = src.FontResourceId;
            Sizes = src.Sizes.Select(sca => sca.Copy()).ToList();
            Clr = src.Clr.Select(sca => sca.Copy()).ToList();
            FontSizes = src.FontSizes.Select(floatKey => floatKey.Copy()).ToList();
            
            Direction = src.Direction;
            WordWrap = src.WordWrap;
            HorizontalAlignment = src.HorizontalAlignment;
            VerticalAlignment = src.VerticalAlignment;
            OverEdge = src.OverEdge;
            UnderEdge = src.UnderEdge;
            LeadingDistribution = src.LeadingDistribution;
        }
    }
}