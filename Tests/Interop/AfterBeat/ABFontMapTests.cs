using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Models.Primitives.Resources;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // AFTERBEAT HAS NO FONT FIELD AT ALL - a typeface is set INLINE, as TextMeshPro's <font> tag
    // written into the string, while this format carries one FontResourceId per object. So the
    // import has to answer a question the source never asked: a string that names three typefaces
    // has to become an object that names one.
    //
    // THE COVERAGE RULE IS WHAT DECIDES IT, and it is the part worth pinning: the object is given
    // the typeface covering MOST of its characters, not the first one written. A <font> run around
    // one word of a paragraph is the ordinary case, and taking the first tag would repaint the whole
    // block in a face the level used for one word - which is not a crash, not a warning, and not
    // something anyone would connect back to an import weeks later.
    //
    // THE TWO TABLES ARE DELIBERATELY NOT ONE. Two source names land on one preset here
    // (Inconsolata and MajorMonoDisplay, Bangers and Poorstory), so the way back has to CHOOSE a
    // canonical name per preset - and two presets Afterbeat inspired nothing in (Play, RubikStorm)
    // are exported under a neighbour's name and therefore do not come home. Both of those are
    // decisions the map's own header states, so they are named here rather than left to look like
    // a round trip that is merely broken.
    //
    // What the tables MAP ONTO is a hand-kept lockstep with this project's own font presets
    // (Assets/Settings/PresetsFonts/*.asset), which the SDK cannot see - so what is checkable here
    // is that every id the export names is a game-defined one and that every name it writes is one
    // the import can read back.

    /// <summary> ABFontMap: the tag's own spelling, the two directions of the pairing, and the
    /// coverage rule that turns several tags into one font. </summary>
    public class ABFontMapTests
    {
        /// <summary> Every preset id the export table answers for - the mirror of
        /// Assets/Settings/PresetsFonts. </summary>
        private static readonly int[] PresetIds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // The two the map's own header calls out: Afterbeat ships nothing distressed and nothing
        // technical, so these export under a neighbour's name and come back as that neighbour.
        private static readonly HashSet<int> ExportedUnderANeighboursName = new() { 4, 10 };

        private static FontResourceId Resolve(string name)
        {
            Assert.IsTrue(ABFontMap.TryResolve(name, out var font), $"'{name}' resolved to nothing");
            return font;
        }

        #region The tag's own spelling

        [TestCase("LiberationSans", "liberationsans")]
        [TestCase("  Anton SDF  ", "anton")]
        [TestCase("\"Oswald Bold SDF\"", "oswald bold")]
        [TestCase("'hellovetica'", "hellovetica")]
        [TestCase("ELECTRONIC HIGHWAY SIGN", "electronic highway sign")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Normalize_FoldsQuotesCaseAndTheAssetsOwnSuffix(string tag, string expected)
        {
            Assert.AreEqual(expected, ABFontMap.Normalize(tag));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Normalize_OfNothing_IsEmptyRatherThanNull(string tag)
        {
            Assert.AreEqual(string.Empty, ABFontMap.Normalize(tag) ?? string.Empty);
        }

        // Old levels write the asset's own name and newer ones the bare one; the source game
        // rewrites the former on load, and a .vgd read here never went through that.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void BothSpellingsOfOneTypeface_ResolveToTheSameFont()
        {
            Assert.AreEqual(Resolve("Anton"), Resolve("Anton SDF"));
            Assert.AreEqual(Resolve("Roboto-Bold"), Resolve("Roboto-Bold SDF"));
        }

        #endregion

        #region The pairing

        [TestCase("liberationsans")]
        [TestCase("roboto-bold")]
        [TestCase("inconsolata")]
        [TestCase("majormonodisplay")]
        [TestCase("anton")]
        [TestCase("oswald bold")]
        [TestCase("bangers")]
        [TestCase("poorstory")]
        [TestCase("electronic highway sign")]
        [TestCase("hellovetica")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void EveryNameTheSourceGameCanLoad_ResolvesToAGameDefinedFont(string name)
        {
            Assert.IsTrue(Resolve(name).IsGameDefined(),
                "a tag resolved onto an id no shipped preset carries");
        }

        // A name outside that vocabulary loaded nothing over there either, so it is not a loss -
        // but it must be reported as unresolved rather than answered with a plausible font.
        [TestCase("Comic Sans MS")]
        [TestCase("liberationsans sdf - outline")]
        [TestCase("")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ANameTheSourceGameCouldNotLoadEither_IsRefused(string name)
        {
            Assert.IsFalse(ABFontMap.TryResolve(name, out _), $"'{name}' resolved to something");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void EveryPreset_ExportsUnderANameTheImportCanReadBack()
        {
            foreach (var id in PresetIds)
            {
                Assert.IsTrue(ABFontMap.TryExport(new FontResourceId(id), out var name),
                    $"preset {id} exports as nothing, so a text carrying it would lose its typeface");
                Assert.IsTrue(ABFontMap.TryResolve(name, out _),
                    $"preset {id} exports as '{name}', which the import refuses - a dead tag");
            }
        }

        // The round trip holds for every preset Afterbeat has a counterpart for, and deliberately
        // does not for the two it has none for. Naming them is what keeps the second case from
        // reading as the first one being broken.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void APresetComesHome_ExceptTheTwoExportedUnderANeighboursName()
        {
            foreach (var id in PresetIds)
            {
                ABFontMap.TryExport(new FontResourceId(id), out var name);
                var home = Resolve(name).value == id;

                Assert.AreEqual(!ExportedUnderANeighboursName.Contains(id), home,
                    $"preset {id} round trips {(home ? "" : "un")}expectedly through '{name}'");
            }
        }

        // A font the level ships itself has no counterpart by definition - the export must say so
        // rather than substitute one of the ten.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AFontTheLevelShipsItself_ExportsAsNothing()
        {
            Assert.IsFalse(ABFontMap.TryExport(new FontResourceId(-1), out var name));
            Assert.IsNull(name);
        }

        #endregion

        #region The coverage rule

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AStringWithNoTags_TakesTheDefaultAndIsNotMixed()
        {
            var selector = new ABFontMap.Selector();
            selector.Count(20);

            Assert.AreEqual(FontResourceId.Default, selector.Resolve(out var mixed));
            Assert.IsFalse(mixed);
            Assert.IsFalse(selector.Recognized);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void OneTagOverTheWholeString_TakesThatFont()
        {
            var selector = new ABFontMap.Selector();
            selector.Push("Anton SDF");
            selector.Count(12);
            selector.Pop();

            Assert.AreEqual(Resolve("anton"), selector.Resolve(out var mixed));
            Assert.IsFalse(mixed);
            Assert.IsTrue(selector.Recognized);
        }

        // THE ONE THIS RULE EXISTS FOR: a one-word run must not repaint the paragraph around it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AShortRunInsideALongString_DoesNotWin()
        {
            var selector = new ABFontMap.Selector();

            selector.Push("Anton SDF");
            selector.Count(4); // one word
            selector.Pop();

            selector.Push("hellovetica");
            selector.Count(60); // the rest of the paragraph
            selector.Pop();

            Assert.AreEqual(Resolve("hellovetica"), selector.Resolve(out var mixed));
            Assert.IsTrue(mixed, "a string using two typefaces did not report as mixed");
        }

        // Ties go to whichever was seen first, which is what keeps an ordinary string obvious.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AnEqualSplit_KeepsWhicheverWasSeenFirst()
        {
            var selector = new ABFontMap.Selector();

            selector.Push("Anton SDF");
            selector.Count(10);
            selector.Pop();

            selector.Push("hellovetica");
            selector.Count(10);
            selector.Pop();

            Assert.AreEqual(Resolve("anton"), selector.Resolve(out _));
        }

        // A tag naming a typeface nothing here knows drew in the CURRENT one over there too, so it
        // is not a change of typeface - and it must not raise Recognized, which is what the import
        // reports the loss by.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AnUnknownTag_KeepsTheTypefaceInForce()
        {
            var selector = new ABFontMap.Selector();

            selector.Push("Anton SDF");
            selector.Push("Comic Sans MS");
            selector.Count(30);
            selector.Pop();
            selector.Pop();

            Assert.AreEqual(Resolve("anton"), selector.Resolve(out var mixed));
            Assert.IsFalse(mixed, "an unrecognized tag counted as a second typeface");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AnUnknownTagAlone_LeavesRecognizedFalse()
        {
            var selector = new ABFontMap.Selector();
            selector.Push("Comic Sans MS");
            selector.Count(10);

            Assert.IsFalse(selector.Recognized);
        }

        // Nesting is TMP's own: a closing tag returns to whatever was in force under it.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ClosingATag_ReturnsToTheOneUnderIt()
        {
            var selector = new ABFontMap.Selector();

            selector.Push("Anton SDF");
            selector.Count(2);

            selector.Push("hellovetica");
            selector.Count(1);
            selector.Pop();

            selector.Count(2); // back under Anton, which now covers four characters against one

            Assert.AreEqual(Resolve("anton"), selector.Resolve(out var mixed));
            Assert.IsTrue(mixed);
        }

        // A string is scanned by a parser that has no reason to be balanced - an unclosed or extra
        // closing tag is ordinary authored text, not a state this may throw on.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void AnUnbalancedTag_IsSurvivedRatherThanThrownOn()
        {
            var selector = new ABFontMap.Selector();

            Assert.DoesNotThrow(() => selector.Pop());

            selector.Push("Anton SDF");
            selector.Count(5);

            Assert.DoesNotThrow(() => selector.Pop());
            Assert.DoesNotThrow(() => selector.Pop());
            Assert.AreEqual(Resolve("anton"), selector.Resolve(out _));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void ARunOfNoCharacters_IsChargedToNobody()
        {
            var selector = new ABFontMap.Selector();

            selector.Push("Anton SDF");
            selector.Count(0);
            selector.Count(-5);
            selector.Pop();

            Assert.AreEqual(FontResourceId.Default, selector.Resolve(out var mixed),
                "an empty run claimed the whole string");
            Assert.IsFalse(mixed);
        }

        #endregion
    }
}
