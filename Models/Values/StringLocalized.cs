using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class StringLocalized : IString, ICopyable<StringLocalized>
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

        IString ICopyable<IString>.Copy() => new StringLocalized(Strings.Select(s => s.Copy()).ToList());
        public StringLocalized Copy() => new(Strings.Select(s => s.Copy()).ToList());
    }
}