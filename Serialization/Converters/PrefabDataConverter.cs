using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Objects;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class PrefabDataConverter : JsonConverterData<IPrefab>
    {
        private readonly CompatibilityService _compatibilityService;

        public PrefabDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetPrefabType(version);
    }
}