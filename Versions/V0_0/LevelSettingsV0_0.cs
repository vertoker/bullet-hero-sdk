using Newtonsoft.Json;

namespace BH.SDK.Versions.V0_0
{
    // ReSharper disable once InconsistentNaming

    [DataVersion(DataDomains.LevelSettings, 0, 0)]
    public class LevelSettingsV0_0
    {
        [JsonProperty("test_fps")]
        public int Framerate { get; set; }
    }
}