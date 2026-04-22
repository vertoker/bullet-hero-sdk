using BHSDK.Models.Enum.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class StringLanguage
    {
        [JsonProperty(Names.Language)]
        public SystemLanguage Language { get; set; }
        
        [JsonProperty(Names.ValueShort)]
        public string Value { get; set; }

        public StringLanguage()
        {
            Language = SystemLanguage.English;
            Value = string.Empty;
        }
        public StringLanguage(SystemLanguage language, string value)
        {
            Language = language;
            Value = value;
        }
    }
}