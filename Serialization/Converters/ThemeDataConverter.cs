using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class ThemeDataConverter : JsonConverterData<ThemeData>
    {
        private readonly CompatibilityService _compatibilityService;

        public ThemeDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => ModelNames.Theme;
        protected override Type GetType(Version version) => _compatibilityService.GetThemeType(version);
    }
}