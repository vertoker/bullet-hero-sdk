using System;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Data
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