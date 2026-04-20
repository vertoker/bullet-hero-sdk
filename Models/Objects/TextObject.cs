using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class TextObject : Object
    {
        public override ObjectType GetModelType() => ObjectType.Text;
        
        [JsonProperty(ModelNames.Color)]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty(ModelNames.Text)]
        public IString Text { get; set; }
        
        [JsonProperty(ModelNames.Font + ModelNames.Name)]
        public string FontName { get; set; }
        
        [JsonProperty(ModelNames.Font + ModelNames.Size)]
        public int FontSize { get; set; }

        public TextObject()
        {
            Clr = new List<Clr>();
            Text = new StringValue(string.Empty);
            FontName = string.Empty;
            FontSize = 10;
        }
        public TextObject(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot, 
            List<Clr> clr, string text, string fontName, int fontSize)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Clr = clr;
            Text = new StringValue(text);
            FontName = fontName;
            FontSize = fontSize;
        }
        public TextObject(int objectId, int parentObjectId, string name, bool visible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot, 
            List<Clr> clr, IString text, string fontName, int fontSize)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Clr = clr;
            Text = text;
            FontName = fontName;
            FontSize = fontSize;
        }
    }
}