using System;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters
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