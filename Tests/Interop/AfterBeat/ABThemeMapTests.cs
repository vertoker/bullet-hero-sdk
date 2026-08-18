using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Themes are the one part of the two formats that already agreed, so this fixture is really
    // asserting that the agreement is still true - if ThemeData's slot layout is ever renumbered,
    // this is what says so before a level's colours quietly move one band over.
    public class ABThemeMapTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_PutsEveryBandWhereThemeDataDocumentsIt()
        {
            var theme = ABThemeMap.Import(ABMockData.CreateTheme());

            Assert.AreEqual(ValueRules.ThemeCount, theme.Matrix.Length);
            AssertHex("040506", theme.Matrix[ABThemeMap.GuiIndex]);
            AssertHex("010203", theme.Matrix[ABThemeMap.BackgroundIndex]);
            AssertHex("070809", theme.Matrix[ABThemeMap.TailIndex]);

            AssertHex("101010", theme.Matrix[ABThemeMap.PlayerStartIndex]);
            AssertHex("202020", theme.Matrix[ABThemeMap.ObjectStartIndex]);
            AssertHex("303030", theme.Matrix[ABThemeMap.EffectStartIndex]);
            AssertHex("404040", theme.Matrix[ABThemeMap.ParallaxStartIndex]);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void RoundTrip_KeepsEveryColour()
        {
            var source = ABMockData.CreateTheme();
            var exported = ABThemeMap.Export(ABThemeMap.Import(source), source.Id);

            Assert.AreEqual(source.Background, exported.Background);
            Assert.AreEqual(source.Gui, exported.Gui);
            Assert.AreEqual(source.GuiAccent, exported.GuiAccent);
            CollectionAssert.AreEqual(source.Players, exported.Players);
            CollectionAssert.AreEqual(source.Objects, exported.Objects);
            CollectionAssert.AreEqual(source.Effects, exported.Effects);
            CollectionAssert.AreEqual(source.Parallax, exported.Parallax);
        }

        // Deriving the id from the source string rather than minting one is what makes "import the
        // .vgt, then import the .vgd that references it" resolve at all.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_TwiceFromTheSameSourceId_ProducesTheSameThemeId()
        {
            var first = ABThemeMap.Import(ABMockData.CreateTheme());
            var second = ABThemeMap.Import(ABMockData.CreateTheme());

            Assert.AreEqual(first.ThemeId, second.ThemeId);
            Assert.IsTrue(first.ThemeId.IsEnabled());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Export_ReportsThatAlphaIsLost()
        {
            var report = new InteropReport();
            var theme = ABThemeMap.Import(ABMockData.CreateTheme());
            theme.Matrix[ABThemeMap.ObjectStartIndex] = new Color4Value(1f, 1f, 1f, 0.5f);

            ABThemeMap.Export(theme, "x", report);
            Assert.AreEqual(InteropSeverity.Dropped, report.Worst);
        }

        [TestCase("FF8000")]
        [TestCase("#FF8000")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryParseHex_AcceptsBothSpellings(string hex)
        {
            Assert.IsTrue(ABThemeMap.TryParseHex(hex, out var color));
            Assert.AreEqual(1f, color.R, 1e-3f);
            Assert.AreEqual(1f, color.A, 1e-6f);
        }

        [TestCase("")]
        [TestCase("FFF")]
        [TestCase("GGGGGG")]
        [TestCase("FF8000FF")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TryParseHex_RejectsAnythingElse(string hex)
            => Assert.IsFalse(ABThemeMap.TryParseHex(hex, out _));

        private static void AssertHex(string expected, Color4Value actual)
            => Assert.AreEqual(expected, ABThemeMap.FormatHex(actual));
    }

    // The colour policy is a decision rather than a conversion, so it gets a fixture of its own:
    // "opaque follows the theme, transparent keeps its alpha" is what an author will notice.
    public class ABColorMapTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_FullyOpaque_StaysAThemeReference()
        {
            var value = ABColorMap.Import(3, 1f, ABPalette.Objects, null);

            Assert.IsInstanceOf<Color4ThemeRef>(value);
            Assert.AreEqual(ABThemeMap.ObjectStartIndex + 3, ((Color4ThemeRef)value).ThemeColorIndex);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Import_SemiTransparent_BecomesALiteralWithTheRightAlpha()
        {
            var report = new InteropReport();
            var theme = ABThemeMap.Import(ABMockData.CreateTheme());

            var value = ABColorMap.Import(0, 0.5f, ABPalette.Objects, theme, report);

            Assert.IsInstanceOf<Color4Value>(value);
            var literal = (Color4Value)value;
            Assert.AreEqual(0.5f, literal.A, 1e-4f);
            Assert.AreEqual(theme.Matrix[ABThemeMap.ObjectStartIndex].R, literal.R, 1e-4f);
            Assert.AreEqual(InteropSeverity.Approximated, report.Worst);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ToThemeIndex_ClampsInsideItsOwnPalette()
        {
            // An out-of-range object colour must not reach the effect band.
            var index = ABColorMap.ToThemeIndex(99, ABPalette.Objects);
            Assert.Less(index, ABThemeMap.ParallaxStartIndex);
            Assert.GreaterOrEqual(index, ABThemeMap.ObjectStartIndex);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Export_ThemeReference_CrossesExactly()
        {
            var report = new InteropReport();
            var color = new Color4ThemeRef(ABThemeMap.ObjectStartIndex + 5);

            var (index, opacity) = ABColorMap.Export(color, ABPalette.Objects, null, report);

            Assert.AreEqual(5, index);
            Assert.AreEqual(1f, opacity, 1e-6f);
            Assert.IsTrue(report.IsClean);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Export_Literal_MatchesTheNearestSlotAndKeepsAlpha()
        {
            var report = new InteropReport();
            var theme = ABThemeMap.Import(ABMockData.CreateTheme());
            var target = theme.Matrix[ABThemeMap.ObjectStartIndex + 4];

            var literal = new Color4Value(target.R, target.G, target.B, 0.25f);
            var (index, opacity) = ABColorMap.Export(literal, ABPalette.Objects, theme, report);

            Assert.AreEqual(4, index);
            Assert.AreEqual(0.25f, opacity, 1e-4f);
            Assert.AreEqual(InteropSeverity.Approximated, report.Worst);
        }
    }
}
