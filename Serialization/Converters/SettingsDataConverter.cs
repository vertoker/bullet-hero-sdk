using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class SettingsDataConverter : JsonConverterData<SettingsData>
    {
        private readonly CompatibilityService _compatibilityService;

        public SettingsDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => Names.Settings;
        protected override Type GetType(Version version) => _compatibilityService.GetSettingsType(version);
    }
}