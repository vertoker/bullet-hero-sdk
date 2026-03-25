using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class LevelDataConverter : JsonConverterData<LevelData>
    {
        private readonly CompatibilityService _compatibilityService;

        public LevelDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => ModelNames.Level;
        protected override Type GetType(Version version) => _compatibilityService.GetLevelType(version);
    }
}