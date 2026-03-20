using System;
using System.Collections.Generic;
using System.IO;
using BHSDK.Serialization.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Serialization
{
    public class SerializationService
    {
        public readonly CompatibilityService CompatibilityService;
        public readonly JsonSerializer Serializer;
        
        public SerializationService() : this(new CompatibilityService())
        {
            
        }
        public SerializationService(CompatibilityService compatibilityService)
        {
            CompatibilityService = compatibilityService;
            
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                Converters = new List<JsonConverter>
                {
                    new LevelDataConverter(compatibilityService),
                    new VersionConverter(),
                    new IntConverter(),
                    new FloatConverter(),
                    new ColorConverter(),
                    new VectorConverter(),
                    new ScreenLimitConverter(),
                },
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
            Serializer = JsonSerializer.Create(settings);
        }
        public SerializationService(CompatibilityService compatibilityService, JsonSerializer serializer)
        {
            CompatibilityService = compatibilityService;
            Serializer = serializer;
        }

        public const string SerializedDataMimeType = "application/unity.bullethero ";
        
        public string Serialize(ILevel level)
        {
            throw new NotImplementedException();
            
            if (level == null) return string.Empty;
            
            using var textWriter = new StringWriter();
            textWriter.Write(SerializedDataMimeType);
            Serializer.Serialize(textWriter, level);
            
            var json = textWriter.ToString();
            // if serializer doesn't write anything - header can't be added, see deserialize realization
            return json.Length == SerializedDataMimeType.Length ? string.Empty : json;

            // var json = JsonConvert.SerializeObject(level, _settings);
            // if (string.IsNullOrEmpty(json)) return json;
            // return SerializedDataMimeType + " " + json;
        }
        
        public bool CanDeserialize(string json)
        {
            throw new NotImplementedException();
            return json != null && json.StartsWith(SerializedDataMimeType);
        }
        public ILevel Deserialize(string json, bool autoUpdate = true)
        {
            throw new NotImplementedException();
            using var stringReader = new StringReader(json);
            
            if (json.StartsWith(SerializedDataMimeType))
                for (var i = 0; i < SerializedDataMimeType.Length; i++)
                    stringReader.Read();
            
            using var jsonTextReader = new JsonTextReader(stringReader);
            var level = (ILevel)Serializer.Deserialize(jsonTextReader, typeof(ILevel));
            // var level = JsonConvert.DeserializeObject<ILevel>(json, _settings);
            
            //if (autoUpdate) level = _compatibilityService.UpdateModel(level);
            return level;
        }
    }
}