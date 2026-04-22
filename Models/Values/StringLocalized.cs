using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class StringLocalized : IString
    {
        [JsonProperty(Names.Strings)]
        public List<StringLanguage> Strings { get; set; }

        public StringLocalized()
        {
            Strings = new List<StringLanguage>();
        }
        public StringLocalized(List<StringLanguage> strings)
        {
            Strings = strings;
        }

        public StringType GetModelType() => StringType.Localized;
        public string Get()
        {
            var language = Application.systemLanguage;
            var str = Strings.FirstOrDefault(str => str.Language == language);
            return str?.Value ?? string.Empty;
        }
    }
}