using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class PrefabDataConverter : JsonConverterData<PrefabData>
    {
        private readonly CompatibilityService _compatibilityService;

        public PrefabDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => ModelNames.Prefab;
        protected override Type GetType(Version version) => _compatibilityService.GetPrefabType(version);
    }
}