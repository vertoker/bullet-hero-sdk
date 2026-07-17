using System;
using System.Collections.Generic;
using System.Linq;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;
using BH.SDK.Serialization.Converters.CustomTypes;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.Data
{
    public class EffectDataConverter : JsonConverterData<IEffect>, IRequiresDefaultSerializer
    {
        private readonly CompatibilityService _compatibilityService;
        private JsonSerializer _defaultSerializer;

        protected override JsonSerializer OverrideValueSerializer => _defaultSerializer;

        public EffectDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override Type GetType(Version version) => _compatibilityService.GetEffectType(version);

        void IRequiresDefaultSerializer.SetDefaultSerializer(JsonSerializer serializer) => _defaultSerializer = serializer;
        IEnumerable<JsonConverter> IRequiresDefaultSerializer.GetExcludedConverters(IReadOnlyList<JsonConverter> allConverters) =>
            allConverters.OfType<ObjectConverter>();
    }
}