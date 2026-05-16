using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class UserSettingsDataConverter : JsonConverterData<UserSettingsData>
    {
        private readonly CompatibilityService _compatibilityService;

        public UserSettingsDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => Names.Settings;
        protected override Type GetType(Version version) => _compatibilityService.GetUserSettingsType(version);
    }
}