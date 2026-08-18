using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Values;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Two different round trips, and they answer different questions.
    //
    // The PARSER round trip (.vgd -> model -> .vgd) is about fidelity to a foreign document: it must
    // keep even the keys this build has never heard of, because the wiki these models came from is
    // openly behind the game. That one is compared as JSON.
    //
    // The CONVERSION round trip (Afterbeat -> this format -> Afterbeat) can never be exact - the two
    // formats disagree about what a level even contains - so it is compared on the STABLE SUBSET:
    // the things both formats have a field for. Asserting more than that would be asserting that
    // nothing was lost, which is false by construction and documented as such.
    public class AfterBeatRoundTripTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Parser_KeysThisBuildDoesNotKnow_SurviveARoundTrip()
        {
            const string json =
                "{" +
                "  \"objects\": [" +
                "    { \"id\": \"a\", \"st\": 1.5, \"future_key\": { \"nested\": [1, 2, 3] } }" +
                "  ]," +
                "  \"some_whole_section_we_never_heard_of\": { \"x\": 7 }" +
                "}";

            var parsed = AfterBeatSerialization.Deserialize<VgdLevel>(json);
            var written = AfterBeatSerialization.Serialize(parsed);

            StringAssert.Contains("future_key", written);
            StringAssert.Contains("some_whole_section_we_never_heard_of", written);
            StringAssert.Contains("nested", written);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Parser_KnownKeys_SurviveARoundTrip()
        {
            var source = AfterBeatMockData.CreateFullLevel();
            var reparsed = AfterBeatSerialization.Deserialize<VgdLevel>(
                AfterBeatSerialization.Serialize(source));

            Assert.AreEqual(source.Objects.Count, reparsed.Objects.Count);
            Assert.AreEqual(source.Themes.Count, reparsed.Themes.Count);
            Assert.AreEqual(source.Prefabs.Count, reparsed.Prefabs.Count);
            Assert.AreEqual(source.PrefabPlacements.Count, reparsed.PrefabPlacements.Count);
            Assert.AreEqual(source.Editor.Bpm.Value, reparsed.Editor.Bpm.Value, 1e-4f);
            Assert.AreEqual(VgdLevel.EventTrackCount, reparsed.Events.Count);

            var sourceObject = source.Objects[0];
            var reparsedObject = reparsed.Objects[0];
            Assert.AreEqual(sourceObject.StartTime, reparsedObject.StartTime, 1e-4f);
            Assert.AreEqual(sourceObject.Move.Keyframes.Count, reparsedObject.Move.Keyframes.Count);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_StableSubset_SurvivesBothDirections()
        {
            var source = AfterBeatMockData.CreateLevel();
            var imported = AfterBeatLevelImporter.Import(source, AfterBeatMockData.CreateMeta(),
                new AfterBeatOptions(60));

            var exported = AfterBeatLevelExporter.Export(imported.Level, imported.Meta);
            Assert.IsNotNull(exported.Level);

            var original = source.Objects[0];
            var returned = exported.Level.Objects.Single();

            Assert.AreEqual(original.StartTime, returned.StartTime, 1e-2f, "lifetime start");
            Assert.AreEqual(original.Shape, returned.Shape, "shape family");
            Assert.AreEqual(original.ObjectType, returned.ObjectType, "hit or not");
            Assert.AreEqual(original.Depth, returned.Depth, "draw order");
            Assert.AreEqual(original.Move.Keyframes.Count, returned.Move.Keyframes.Count);

            Assert.AreEqual(1, exported.Level.Themes.Count);
            Assert.AreEqual(1, exported.Level.Markers.Count);
            Assert.AreEqual(1, exported.Level.Checkpoints.Count);
            Assert.AreEqual(source.Editor.Bpm.Value, exported.Level.Editor.Bpm.Value, 1e-3f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_CheckpointPosition_SurvivesBothDirections()
        {
            var source = AfterBeatMockData.CreateLevel();
            var imported = AfterBeatLevelImporter.Import(source, null, new AfterBeatOptions(60));
            var exported = AfterBeatLevelExporter.Export(imported.Level, null);

            var returned = exported.Level.Checkpoints.Single();
            Assert.AreEqual(3f, returned.Position.X, 1e-3f);
            Assert.AreEqual(-4f, returned.Position.Y, 1e-3f);
        }

        // Rotation is the one track where a mistake in either direction cancels out under a naive
        // round trip, so it is checked against the ORIGINAL deltas rather than against itself.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Conversion_Rotation_ComesBackAsTheSameDeltas()
        {
            var source = new VgdLevel();
            source.Objects.Add(AfterBeatMockData.CreateRotatingObject());

            var imported = AfterBeatLevelImporter.Import(source, null, new AfterBeatOptions(60));
            var exported = AfterBeatLevelExporter.Export(imported.Level, null);

            var returned = exported.Level.Objects.Single().Rotate.Keyframes
                .OrderBy(k => k.Time)
                .Select(k => k.GetValue(0))
                .ToArray();

            Assert.AreEqual(2, returned.Length);
            Assert.AreEqual(90f, returned[0], 1e-2f);
            Assert.AreEqual(90f, returned[1], 1e-2f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AudioAndEffects_AreReportedAsLost()
        {
            var imported = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null,
                new AfterBeatOptions(60));

            imported.Level.Resources.Effects[new Models.Primitives.EffectId(System.Guid.NewGuid())] =
                new Models.Data.EffectData();

            var exported = AfterBeatLevelExporter.Export(imported.Level, null);
            var codes = exported.Report.Issues.Select(i => i.Code).ToArray();

            CollectionAssert.Contains(codes, "effect_resources");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_InactiveObject_IsSkippedAndReported()
        {
            var imported = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null,
                new AfterBeatOptions(60));
            foreach (var pair in imported.Level.Game.Objects) pair.Value.Active = false;

            var exported = AfterBeatLevelExporter.Export(imported.Level, null);

            Assert.IsEmpty(exported.Level.Objects);
            CollectionAssert.Contains(exported.Report.Issues.Select(i => i.Code).ToArray(), "inactive_objects");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Interop_ThemeFile_RoundTripsThroughText()
        {
            var themeJson = AfterBeatSerialization.Serialize(AfterBeatMockData.CreateTheme());

            var theme = AfterBeatInterop.ImportTheme(themeJson);
            Assert.IsNotNull(theme);

            var written = AfterBeatInterop.ExportTheme(theme);
            var reparsed = AfterBeatSerialization.Deserialize<VgtTheme>(written);

            CollectionAssert.AreEqual(
                AfterBeatMockData.CreateTheme().Objects, reparsed.Objects);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Interop_PrefabFile_RoundTripsThroughText()
        {
            var prefab = new VgpPrefab { Id = "p1", Name = "Burst" };
            prefab.Objects.Add(AfterBeatMockData.CreateObject("inner"));

            var imported = AfterBeatInterop.ImportPrefab(AfterBeatSerialization.Serialize(prefab));
            Assert.IsNotNull(imported);
            Assert.AreEqual(1, imported.Objects.Count);

            var reparsed = AfterBeatSerialization.Deserialize<VgpPrefab>(
                AfterBeatInterop.ExportPrefab(imported));
            Assert.AreEqual(1, reparsed.Objects.Count);
            Assert.AreEqual("Burst", reparsed.Name);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Interop_ExportLevel_ProducesBothDocuments()
        {
            var imported = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(),
                AfterBeatMockData.CreateMeta(), new AfterBeatOptions(60));

            var exported = AfterBeatInterop.ExportLevel(imported.Level, imported.Meta);

            Assert.IsNotEmpty(exported.LevelJson);
            Assert.IsNotEmpty(exported.MetaJson);
            StringAssert.Contains("Test Song", exported.MetaJson);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Export_KeepsTheLevelsOwnFramerate_NotTheCallers()
        {
            var imported = AfterBeatLevelImporter.Import(AfterBeatMockData.CreateLevel(), null,
                new AfterBeatOptions(30));

            // Frames are being turned back into seconds; reading them at 120 would retime the level.
            var exported = AfterBeatLevelExporter.Export(imported.Level, null, new AfterBeatOptions(120));

            Assert.AreEqual(1f, exported.Level.Objects.Single().StartTime, 1e-2f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Report_AggregatesByCause_RatherThanAccumulating()
        {
            var report = new BH.SDK.Interop.InteropReport();
            for (var i = 0; i < 500; i++) report.Dropped("same_cause", "message", $"objects[{i}]");

            Assert.AreEqual(1, report.Issues.Count);
            Assert.AreEqual(500, report.Issues[0].Count);
            Assert.AreEqual("objects[0]", report.Issues[0].FirstPath, "the first one is the one to go look at");
        }
    }
}
