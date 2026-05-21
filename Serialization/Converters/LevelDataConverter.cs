using System;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters
{
    public class LevelDataConverter : JsonConverterData<ILevel>
    {
        private readonly CompatibilityService _compatibilityService;

        public LevelDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetLevelType(version);
    }
}