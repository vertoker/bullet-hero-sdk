using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class PlayerSettingsDataConverter : JsonConverterData<PlayerSettingsData>
    {
        private readonly CompatibilityService _compatibilityService;

        public PlayerSettingsDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => Names.PlayerSettings;
        protected override Type GetType(Version version) => _compatibilityService.GetPlayerSettingsType(version);
    }
}