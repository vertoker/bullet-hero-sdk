using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Values;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // A level that never authored a palette of its own stores nothing but an index - "19" - and its
    // themes[] stays empty. Before the shipped table existed that was a dangling reference, so the
    // whole level imported with no theme and every theme-referenced colour resolved to white. Every
    // test here is about that one path.
    //
    // The colour assertions are spot checks against the shipped game's own data rather than a full
    // transcription: the table is 714 colours, and a test that restates all of them tests the
    // copy-paste, not the code.
    public class ABDefaultThemesTests
    {
        private const int Framerate = 60;

        private static VgdLevel LevelUsingTheme(string sourceId)
        {
            var level = new VgdLevel();
            level.Objects.Add(ABMockData.CreateObject());
            AddThemeKey(level, sourceId, 0f);
            return level;
        }

        private static void AddThemeKey(VgdLevel level, string sourceId, float time)
        {
            var key = new VgdEventKeyframe { Time = time };
            key.SetString(sourceId);
            level.Events[(int)ABEventTrack.Theme].Add(key);
        }

        #region The table

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Table_HoldsEveryShippedTheme_AddressedByItsIndex()
        {
            Assert.AreEqual(ABDefaultThemes.Count, ABDefaultThemes.All().Count());

            for (var index = 0; index < ABDefaultThemes.Count; index++)
            {
                var id = index.ToString();
                Assert.IsTrue(ABDefaultThemes.Contains(id), $"theme {id} is missing");
                Assert.AreEqual(id, ABDefaultThemes.Get(id).Id);
            }

            Assert.IsFalse(ABDefaultThemes.Contains(ABDefaultThemes.Count.ToString()),
                "the source game ships no theme past the last index");
            Assert.IsNull(ABDefaultThemes.Get("not-an-index"));
        }

        // Two rows at opposite ends of the table, and one field of each that a shifted parse would
        // get wrong: the id is the only thing addressing a theme, and the accent is the last colour
        // before the four lists.
        [TestCase("0", "Machine", "94D8DB", "EF5350")]
        [TestCase("10", "Desert Heat", "FCE8C7", "EF5350")]
        [TestCase("20", "HotPanda", "27232A", "C80224")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Table_SpotChecksAgainstTheShippedGame(string id, string name, string background, string accent)
        {
            var theme = ABDefaultThemes.Get(id);

            Assert.AreEqual(name, theme.Name);
            Assert.AreEqual(background, theme.Background);
            Assert.AreEqual(accent, theme.GuiAccent);
        }

        // The object list is the one that VARIES in length, so a parser assuming nine would either
        // throw or invent colours. Both lengths below are what the game holds.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Table_KeepsShortObjectLists_AsShort()
        {
            Assert.AreEqual(2, ABDefaultThemes.Get("7").Objects.Count, "Black/White");
            Assert.AreEqual(9, ABDefaultThemes.Get("19").Objects.Count, "Wonderland");

            foreach (var theme in ABDefaultThemes.All())
            {
                Assert.AreEqual(VgtTheme.PlayerCount, theme.Players.Count, theme.Name);
                Assert.AreEqual(VgtTheme.ParallaxCount, theme.Parallax.Count, theme.Name);
                Assert.AreEqual(VgtTheme.EffectCount, theme.Effects.Count, theme.Name);
                Assert.LessOrEqual(theme.Objects.Count, VgtTheme.ObjectCount, theme.Name);
            }
        }

        #endregion

        #region Import

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ALevelNamingOnlyAShippedTheme_MaterializesItAsItsOwn()
        {
            var result = ABLevelImporter.Import(LevelUsingTheme("19"), null,
                new ABOptions(Framerate));

            Assert.AreEqual(1, result.Level.Resources.Themes.Count);

            var theme = result.Level.Resources.Themes.Values.Single();
            Assert.AreEqual("Wonderland", theme.Name);
            Assert.AreEqual(ABIdMap.ToThemeId("19"), theme.ThemeId,
                "the id the theme track names has to resolve to the theme that was materialized");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_TheMaterializedTheme_CarriesTheShippedColoursInTheirSlots()
        {
            var result = ABLevelImporter.Import(LevelUsingTheme("19"), null,
                new ABOptions(Framerate));

            var matrix = result.Level.Resources.Themes.Values.Single().Matrix;

            ABThemeMap.TryParseHex("FFAAFF", out var background);
            ABThemeMap.TryParseHex("FD2FAB", out var firstObject);

            Assert.AreEqual(background, matrix[ABThemeMap.BackgroundIndex]);
            Assert.AreEqual(firstObject, matrix[ABThemeMap.ObjectStartIndex]);
        }

        // The reference theme is what every literal colour in the level is resolved against, so a
        // level using nothing but a shipped palette used to resolve all of them against nothing.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ALevelNamingOnlyAShippedTheme_StillSelectsItOnTheTrack()
        {
            var result = ABLevelImporter.Import(LevelUsingTheme("19"), null,
                new ABOptions(Framerate));

            var key = result.Level.Game.Events.Themes.Single();
            Assert.IsTrue(result.Level.Resources.Themes.ContainsKey(key.ThemeId));
        }

        // An id the level does not define and the game does not ship is the one case with nothing to
        // fall back on - it is reported rather than quietly turned into some other theme.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnUnknownThemeReference_IsReported()
        {
            var result = ABLevelImporter.Import(LevelUsingTheme("not-a-theme"), null,
                new ABOptions(Framerate));

            Assert.IsEmpty(result.Level.Resources.Themes);
            Assert.IsTrue(result.Report.Issues.Any(issue => issue.Code == "theme_reference_unknown"));
        }

        // A level carrying its own palette AND naming a shipped one gets both, and the one it starts
        // on is the one literal colours resolve against - not whichever happens to be written first.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_ACustomAndAShippedTheme_KeepsBothAndStartsOnTheEarlierKeyframe()
        {
            var level = LevelUsingTheme("10");
            level.Themes.Add(ABMockData.CreateTheme());
            AddThemeKey(level, ABMockData.ThemeSourceId, 5f);

            var result = ABLevelImporter.Import(level, null, new ABOptions(Framerate));

            Assert.AreEqual(2, result.Level.Resources.Themes.Count);
            Assert.IsTrue(result.Level.Resources.Themes.ContainsKey(ABIdMap.ToThemeId("10")));
            Assert.IsTrue(result.Level.Resources.Themes
                .ContainsKey(ABIdMap.ToThemeId(ABMockData.ThemeSourceId)));

            var keys = result.Level.Game.Events.Themes.OrderBy(key => key.Frame).ToArray();
            Assert.AreEqual(ABIdMap.ToThemeId("10"), keys[0].ThemeId,
                "the shipped theme is the one the level opens on");
        }

        #endregion
    }
}
