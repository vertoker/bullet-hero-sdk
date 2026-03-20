using System.IO;
using BHSDK.Models;
using BHSDK.Serialization;
using BHSDK.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace BHSDK.Tests.Serialization
{
    public class SerializationTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestSerialization()
        {
            var serializationService = new SerializationService();

            var level = new Level();
            var levelData = new LevelData(level);

            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, levelData);
            var json = textWriter.ToString();
            Debug.Log($"<color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            levelData = serializationService.Serializer.Deserialize<LevelData>(reader);
        }
    }
}