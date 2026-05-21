using System;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters
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