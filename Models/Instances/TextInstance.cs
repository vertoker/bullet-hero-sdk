using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class TextInstance : Instance
    {
        [JsonProperty("clr")]
        public List<Clr> Clr { get; set; }
        
        [JsonProperty("t")]
        public string Text { get; set; }
        
        [JsonProperty("fn")]
        public string FontName { get; set; }
        
        [JsonProperty("fs")]
        public int FontSize { get; set; }

        public TextInstance()
        {
            Clr = new List<Clr>();
            Text = string.Empty;
            FontName = string.Empty;
            FontSize = 10;
        }
        public TextInstance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot, 
            List<Clr> clr, string text, string fontName, int fontSize)
            : base(instanceId, parentInstanceId, name, isVisible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Clr = clr;
            Text = text;
            FontName = fontName;
            FontSize = fontSize;
        }
    }
}