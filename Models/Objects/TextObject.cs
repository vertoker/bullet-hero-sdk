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
        
        [JsonProperty(ModelNames.Size)]
        public List<Sca> Sizes { get; set; }
        
        [JsonProperty(ModelNames.Color)]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty(ModelNames.Font + ModelNames.Size)]
        public List<FloatKey> FontSizes { get; set; }
        
        [JsonProperty(ModelNames.Text)]
        public IString Text { get; set; }
        
        // positive with 0 - game-defined (0 is NotoSans), negative - user-defined
        // more about resourceId and how it works, read in Resource.cs file
        
        [JsonProperty(ModelNames.Font + ModelNames.Resource + ModelNames.Id)]
        public int FontResourceId { get; set; }
        
        [JsonProperty(ModelNames.Direction)]
        public TextObjectDirection Direction { get; set; }
        
        [JsonProperty(ModelNames.Word + ModelNames.Wrap)]
        public bool WordWrap { get; set; }
        
        [JsonProperty(ModelNames.Horizontal + ModelNames.Alignment)]
        public TextObjectHorizontalAlignment HorizontalAlignment { get; set; }
        
        [JsonProperty(ModelNames.Vertical + ModelNames.Alignment)]
        public TextObjectVerticalAlignment VerticalAlignment { get; set; }
        
        [JsonProperty(ModelNames.Over)]
        public TextObjectOverEdge OverEdge { get; set; }
        
        [JsonProperty(ModelNames.Under)]
        public TextObjectUnderEdge UnderEdge { get; set; }
        
        [JsonProperty(ModelNames.Distribution)]
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