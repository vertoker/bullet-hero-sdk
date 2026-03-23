using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace BHSDK.Serialization
{
    public static class SerializationUtils
    {
        public const string TypePropertyName = "t";
        public const string ValuePropertyName = "v";
        
        public static void SerializationErrorHandling(object sender, ErrorEventArgs args)
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
            else
            {
                Debug.LogException(args.ErrorContext.Error);
            }
        }
    }
}