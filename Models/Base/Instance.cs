using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Base
{
    public class Instance
    {
        public virtual InstanceType GetModelType() => InstanceType.Base;
        
        [JsonProperty(ModelNames.InstanceId)]
        public int InstanceId { get; set; }
        
        [JsonProperty(ModelNames.ParentInstanceId)]
        public int ParentInstanceId { get; set; }
        
        [JsonProperty(ModelNames.Name)]
        public string Name { get; set; }
        
        [JsonProperty(ModelNames.IsVisible)]
        public bool IsVisible { get; set; }
        
        
        [JsonProperty(ModelNames.Start + ModelNames.Frame)]
        public int StartFrame { get; set; }
        
        [JsonProperty(ModelNames.End + ModelNames.Frame)]
        public int EndFrame { get; set; }
        
        [JsonProperty(ModelNames.Position)]
        public List<Pos> Pos { get; set; }
        
        [JsonProperty(ModelNames.Rotation)]
        public List<Rot> Rot { get; set; }
        
        [JsonProperty(ModelNames.Scale)]
        public List<Sca> Sca { get; set; }
        
        [JsonProperty(ModelNames.Layer)]
        public int Layer { get; set; }
        
        [JsonProperty(ModelNames.Pivot)]
        public Anchor Pivot { get; set; }

        public Instance()
        {
            InstanceId = 0;
            ParentInstanceId = 0;
            Name = string.Empty;
            IsVisible = true;
            
            StartFrame = 0;
            EndFrame = 0;
            Pos = new List<Pos>();
            Rot = new List<Rot>();
            Sca = new List<Sca>();
            Layer = 0;
            Pivot = Anchor.Center_Middle;
        }
        public Instance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot)
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
            Layer = layer;
            Pivot = pivot;
        }
    }
}