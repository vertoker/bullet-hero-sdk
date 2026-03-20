using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Instances;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class TextInstance : IInstance
    {
        [JsonProperty("id")]
        public int InstanceId { get; set; }
        
        [JsonProperty("pid")]
        public int ParentInstanceId { get; set; }
        
        [JsonProperty("n")]
        public string Name { get; set; }
        
        [JsonProperty("v")]
        public bool IsVisible { get; set; }
        
        
        [JsonProperty("sf")]
        public int StartFrame { get; set; }
        
        [JsonProperty("ef")]
        public int EndFrame { get; set; }
        
        [JsonProperty("pos")]
        public List<PosModel> Pos { get; set; }
        
        [JsonProperty("rot")]
        public List<RotModel> Rot { get; set; }
        
        [JsonProperty("sca")]
        public List<ScaModel> Sca { get; set; }
        
        [JsonProperty("clr")]
        public List<ClrModel> Clr { get; set; }
        
        
        [JsonProperty("t")]
        public string Text { get; set; }
        
        [JsonProperty("fn")]
        public string FontName { get; set; }
        
        [JsonProperty("fs")]
        public int FontSize { get; set; }
        
        [JsonProperty("a")]
        public Anchor Alignment { get; set; }

        public TextInstance()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            Name = string.Empty;
            IsVisible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<PosModel>();
            Rot = new List<RotModel>();
            Sca = new List<ScaModel>();
            Clr = new List<ClrModel>();
            
            Text = string.Empty;
            FontName = string.Empty;
            FontSize = 10;
            Alignment = Anchor.Center_Middle;
        }
        public TextInstance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<PosModel> pos, List<RotModel> rot, List<ScaModel> sca, List<ClrModel> clr, 
            string text, string fontName, int fontSize, Anchor alignment)
        {
            InstanceId = instanceId;
            ParentInstanceId = parentInstanceId;
            Name = name;
            IsVisible = isVisible;
            StartFrame = startFrame;
            EndFrame = endFrame;
            Pos = pos;
            Rot = rot;
            Sca = sca;
            Clr = clr;
            Text = text;
            FontName = fontName;
            FontSize = fontSize;
            Alignment = alignment;
        }
    }
}