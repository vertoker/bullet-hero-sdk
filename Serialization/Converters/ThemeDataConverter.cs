using System;
using BHSDK.Models;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class ThemeDataConverter : JsonConverterData<ITheme>
    {
        private readonly CompatibilityService _compatibilityService;

        public ThemeDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
        
        protected override Type GetType(Version version) => _compatibilityService.GetThemeType(version);
    }
}