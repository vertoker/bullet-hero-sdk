using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class EffectDataConverter : JsonConverterData<EffectData>
    {
        private readonly CompatibilityService _compatibilityService;

        protected override JsonSerializer OverrideValueSerializer { get; }
        
        public EffectDataConverter(CompatibilityService compatibilityService, JsonSerializer serializerDefault)
        {
            _compatibilityService = compatibilityService;
            OverrideValueSerializer = serializerDefault;
        }

        protected override string GetObjectPropertyName() => ModelNames.Effect;
        protected override Type GetType(Version version) => _compatibilityService.GetEffectType(version);
    }
}