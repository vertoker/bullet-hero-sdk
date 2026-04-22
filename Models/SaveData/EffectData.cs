using System;
using BHSDK.Models.Interfaces.SaveData;
using Newtonsoft.Json;

namespace BHSDK.Models.SaveData
{
    public class EffectData : ISaveData
    {
        [JsonProperty(Names.Version)]
        public Version Version { get; set; }
        
        [JsonProperty(Names.Effect)]
        public IEffect Effect { get; set; }

        public IData GetData() => Effect;
        public void SetData(object data) => Effect = data as IEffect;

        public EffectData()
        {
            
        }
        public EffectData(IEffect effect)
        {
            Version = effect.GetVersion();
            Effect = effect;
        }
    }
}