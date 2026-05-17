using System;
using BHSDK.Models;
using BHSDK.Models.Objects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class EffectDataConverter : JsonConverterData<EffectObject>
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