using System;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class LevelMetaDataConverter : JsonConverterData<ILevelMeta>
    {
        private readonly CompatibilityService _compatibilityService;

        public LevelMetaDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetLevelMetaType(version);
    }
}