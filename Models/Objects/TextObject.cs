using System.Collections.Generic;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Text;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class TextObject : Object
    {
        public override ObjectType GetModelType() => ObjectType.Text;
        
        [JsonProperty(Names.Size)]
        public List<Sca> Sizes { get; set; }
        
        [JsonProperty(Names.Color)]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty(Names.FontSize)]
        public List<FloatKey> FontSizes { get; set; }
        
        [JsonProperty(Names.Text)]
        public IString Text { get; set; }
        
        // positive with 0 - game-defined (0 is NotoSans), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(Names.FontResourceId)]
        public int FontResourceId { get; set; }
        
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
            Sizes = new List<Sca>();
            Clr = new List<Clr>();
            FontSizes = new List<FloatKey>();
            Text = new StringValue(string.Empty);
            FontResourceId = 0;
            
            Direction = TextStatic.DirectionDefault;
            WordWrap = TextStatic.WordWrapDefault;
            HorizontalAlignment = TextStatic.HorizontalAlignmentDefault;
            VerticalAlignment = TextStatic.VerticalAlignmentDefault;
            OverEdge = TextStatic.OverEdgeDefault;
            UnderEdge = TextStatic.UnderEdgeDefault;
            LeadingDistribution = TextStatic.LeadingDistributionDefault;
        }
        public TextObject(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame, 
            List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot, List<Sca> sizes, List<Clr> clr, 
            List<FloatKey> fontSizes, IString text, int fontResourceId, TextObjectDirection direction, bool wordWrap, 
            TextObjectHorizontalAlignment horizontalAlignment, TextObjectVerticalAlignment verticalAlignment, 
            TextObjectOverEdge overEdge, TextObjectUnderEdge underEdge, TextObjectLeadingDistribution leadingDistribution)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Sizes = sizes;
            Clr = clr;
            FontSizes = fontSizes;
            Text = text;
            FontResourceId = fontResourceId;
            
            Direction = direction;
            WordWrap = wordWrap;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            OverEdge = overEdge;
            UnderEdge = underEdge;
            LeadingDistribution = leadingDistribution;
        }
    }
}