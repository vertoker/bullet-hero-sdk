using System;
using BHSDK.Models;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class LevelDataConverter : JsonConverterData<Level>
    {
        private readonly CompatibilityService _compatibilityService;

        public LevelDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetLevelType(version);
    }
}