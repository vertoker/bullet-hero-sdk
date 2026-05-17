using System;
using BHSDK.Models;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class UserSettingsDataConverter : JsonConverterData<UserSettings>
    {
        private readonly CompatibilityService _compatibilityService;

        public UserSettingsDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetUserSettingsType(version);
    }
}