using System;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Data
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