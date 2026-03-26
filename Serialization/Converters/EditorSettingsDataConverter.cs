using System;
using BHSDK.Models;
using BHSDK.Models.SaveData;
using BHSDK.Serialization.Converters.Base;

namespace BHSDK.Serialization.Converters
{
    public class EditorSettingsDataConverter : JsonConverterData<EditorSettingsData>
    {
        private readonly CompatibilityService _compatibilityService;

        public EditorSettingsDataConverter(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        protected override string GetObjectPropertyName() => ModelNames.EditorSettings;
        protected override Type GetType(Version version) => _compatibilityService.GetEditorSettingsType(version);
    }
}