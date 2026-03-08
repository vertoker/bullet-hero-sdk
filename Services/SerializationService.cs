using System.IO;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Services
{
    public class SerializationService
    {
        public const string SerializedDataMimeType = "application/unity.bullethero ";
        
        private readonly CompatibilityService _compatibilityService;
        private readonly JsonSerializerSettings _settings;
        private readonly JsonSerializer _serializer;
        
        public SerializationService()
        {
            _compatibilityService = new CompatibilityService();
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                Error = (sender, args) =>
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        // Created specially for Android ADB console, this format is more readable (hello Pico 4)
                        Debug.LogError($"sender: {sender}");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"object: {args.CurrentObject}");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"error: {args.ErrorContext.Error}, ");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"member: {args.ErrorContext.Member}, ");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"Path: {args.ErrorContext.Path}, ");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"OriginalObject: {args.ErrorContext.OriginalObject}, ");
                        Debug.Log("***************************************************");
                    }
                }
            };
            _serializer = JsonSerializer.Create(_settings);
        }
        public SerializationService(CompatibilityService compatibilityService, 
            JsonSerializerSettings settings, JsonSerializer serializer)
        {
            _compatibilityService = compatibilityService;
            _settings = settings;
            _serializer = serializer;
        }

        public string Serialize(ILevel level)
        {
            if (level == null) return string.Empty;
            
            using var textWriter = new StringWriter();
            textWriter.Write(SerializedDataMimeType);
            _serializer.Serialize(textWriter, level);
            
            var json = textWriter.ToString();
            // if serializer doesn't write anything - header can't be added, see deserialize realization
            return json.Length == SerializedDataMimeType.Length ? string.Empty : json;

            // var json = JsonConvert.SerializeObject(level, _settings);
            // if (string.IsNullOrEmpty(json)) return json;
            // return SerializedDataMimeType + " " + json;
        }
        
        public bool CanDeserialize(string json)
        {
            return json != null && json.StartsWith(SerializedDataMimeType);
        }
        public ILevel Deserialize(string json, bool autoUpdate = true)
        {
            using var stringReader = new StringReader(json);
            
            if (json.StartsWith(SerializedDataMimeType))
                for (var i = 0; i < SerializedDataMimeType.Length; i++)
                    stringReader.Read();
            
            using var jsonTextReader = new JsonTextReader(stringReader);
            var level = (ILevel)_serializer.Deserialize(jsonTextReader, typeof(ILevel));
            // var level = JsonConvert.DeserializeObject<ILevel>(json, _settings);
            
            if (autoUpdate) level = _compatibilityService.UpdateModel(level);
            return level;
        }
    }
}