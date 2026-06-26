using System;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.Data
{
    public class EffectDataConverter : JsonConverterData<IEffect>
    {
        private readonly CompatibilityService _compatibilityService;

        protected override JsonSerializer OverrideValueSerializer { get; }
        
        public EffectDataConverter(CompatibilityService compatibilityService, JsonSerializer serializerDefault)
        {
            _compatibilityService = compatibilityService;
            OverrideValueSerializer = serializerDefault;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetEffectType(version);
    }
}