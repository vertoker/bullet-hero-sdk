using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class UserSettingsDataConverter : JsonConverterData<IUserSettings>
    {
        private readonly CompatibilityService _compatibilityService;

        public UserSettingsDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetUserSettingsType(version);
    }
}