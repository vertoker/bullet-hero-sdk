using BH.SDK.Models;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Serialization;
using BH.SDK.Serialization.Serializers;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    // The three serialize modes are what the editor WRITES with, and all three default to Json for a
    // reason worth pinning: Json is 0, so a settings file written before they existed deserializes
    // into the same value a fresh one is born with, and no migration was needed. The Copy/Pull/Equals
    // boilerplate here is hand-written eleven fields over, which is the mistake this file exists for.

    /// <summary> GameEditorSettings' serialize-mode preferences: defaults, boilerplate, round trip. </summary>
    public class GameEditorSettingsTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void SerializeModes_DefaultToJson()
        {
            var settings = new GameEditorSettings();

            Assert.AreEqual(SerializationType.Json, settings.LevelSerializeMode);
            Assert.AreEqual(SerializationType.Json, settings.ResourcesSerializeMode);
            Assert.AreEqual(SerializationType.Json, settings.CopySerializeMode);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Reset_RestoresSerializeModes()
        {
            var settings = new GameEditorSettings
            {
                LevelSerializeMode = SerializationType.Bson,
                ResourcesSerializeMode = SerializationType.JsonPretty,
                CopySerializeMode = SerializationType.Bson,
            };

            settings.Reset();

            Assert.AreEqual(SerializationType.Json, settings.LevelSerializeMode);
            Assert.AreEqual(SerializationType.Json, settings.ResourcesSerializeMode);
            Assert.AreEqual(SerializationType.Json, settings.CopySerializeMode);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void CopyAndPull_CarrySerializeModes()
        {
            var source = new GameEditorSettings
            {
                LevelSerializeMode = SerializationType.JsonPretty,
                ResourcesSerializeMode = SerializationType.Bson,
                CopySerializeMode = SerializationType.JsonPretty,
            };

            var copy = source.Copy();
            Assert.IsTrue(source.Equals(copy));

            var pulled = new GameEditorSettings();
            pulled.Pull(source);
            Assert.IsTrue(source.Equals(pulled));
            Assert.AreEqual(source.GetHashCode(), pulled.GetHashCode());
        }

        // Each mode is its own field, so a Copy/Pull that folded two of them together would still pass
        // the test above - this is what tells them apart.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Equals_SeesEachSerializeModeIndependently()
        {
            var a = new GameEditorSettings();

            var b = a.Copy();
            b.LevelSerializeMode = SerializationType.Bson;
            Assert.IsFalse(a.Equals(b));

            var c = a.Copy();
            c.ResourcesSerializeMode = SerializationType.Bson;
            Assert.IsFalse(a.Equals(c));

            var d = a.Copy();
            d.CopySerializeMode = SerializationType.Bson;
            Assert.IsFalse(a.Equals(d));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void SerializeModes_SurviveARoundTrip()
        {
            var service = new SerializationService(new SerializationSettings());

            var settings = new UserSettings();
            settings.GameEditor.LevelSerializeMode = SerializationType.JsonPretty;
            settings.GameEditor.ResourcesSerializeMode = SerializationType.Bson;
            settings.GameEditor.CopySerializeMode = SerializationType.JsonPretty;

            var json = service.SerializeData(settings);
            var restored = service.DeserializeData<UserSettings>(json);

            Assert.AreEqual(SerializationType.JsonPretty, restored.GameEditor.LevelSerializeMode);
            Assert.AreEqual(SerializationType.Bson, restored.GameEditor.ResourcesSerializeMode);
            Assert.AreEqual(SerializationType.JsonPretty, restored.GameEditor.CopySerializeMode);
        }
    }
}
