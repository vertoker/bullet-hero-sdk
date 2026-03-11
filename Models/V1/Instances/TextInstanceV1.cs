using System.Collections.Generic;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Components;
using BHSDK.Models.Interfaces.Instances;

namespace BHSDK.Models.V1.Instances
{
    public class TextInstanceV1 : ITextInstance
    {
        public int InstanceId { get; set; }
        public int ParentInstanceId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public List<IPos> Pos { get; set; }
        public List<IRot> Rot { get; set; }
        public List<ISca> Sca { get; set; }
        public List<IClr> Clr { get; set; }
        
        public string Text { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public Anchor Alignment { get; set; }

        public TextInstanceV1()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            Name = string.Empty;
            IsVisible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<IPos>();
            Rot = new List<IRot>();
            Sca = new List<ISca>();
            Clr = new List<IClr>();
            
            Text = string.Empty;
            FontName = string.Empty;
            FontSize = 10;
            Alignment = Anchor.Center_Middle;
        }
        public TextInstanceV1(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<IPos> pos, List<IRot> rot, List<ISca> sca, List<IClr> clr, 
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