using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Components;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class EffectInstance : Instance
    {
        [JsonProperty("c")]
        public bool HasCollider { get; set; }

        public EffectInstance()
        {
            HasCollider = false;
        }

        public EffectInstance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, bool hasCollider)
            : base(instanceId, parentInstanceId, name, isVisible, startFrame, endFrame, pos, rot, sca)
        {
            HasCollider = hasCollider;
        }
    }
}