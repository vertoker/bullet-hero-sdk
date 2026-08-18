using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Draw order is the conversion with the most ways to be silently wrong: every one of these
    // failures still produces a level that loads, plays, and looks plausible until the moment two
    // objects that should not overlap do.
    //
    // The invariants worth stating once, since almost every test below is one of them:
    //
    //   THE PLAYER LINE - this format draws its avatar at layer -0.5, Afterbeat draws its player
    //   between depth 0 and depth 1, so an object at depth 0 must land at layer >= 0 and one at any
    //   other depth at layer <= -1. The mapping this replaced put depth 20 on layer 0, i.e. drew
    //   almost every object of an ordinary level in front of the player.
    //
    //   THE BANDS never interleave: everything AbovePlayer is in front of everything Default, which
    //   is in front of everything Background, whatever depths any of them carry.
    //
    //   AUTO IS ORDER-PRESERVING - it may compress, never reorder. Anything OnlyDepth draws in
    //   front, Auto draws in front or level with.
    public class ABLayerMapTests
    {
        #region Fixture

        private static VgdObject Obj(int depth, ABRenderLayer band = ABRenderLayer.Default,
            int editorLayer = 0, int editorBin = 0, string id = null)
            => new()
            {
                Id = id ?? $"d{depth}-b{(int)band}-l{editorLayer}-n{editorBin}",
                Depth = depth,
                RenderLayer = (int)band,
                Editor = new VgdObjectEditor { Layer = editorLayer, Bin = editorBin },
            };

        private static ABOptions Options(ABLayerImport mode)
            => new() { LayerImport = mode };

        private static int[] Resolve(IReadOnlyList<VgdObject> sources, ABLayerImport mode,
            InteropReport report = null)
            => ABLayerMap.Resolve(sources, Options(mode), report).Layers;

        #endregion

        #region OnlyDepth

        // The whole band layout in one case, because the three bands are only correct relative to
        // each other: 61 depths each, back to back, Default's frontmost sitting on layer 0.
        [TestCase(ABRenderLayer.Default, 0, 0)]
        [TestCase(ABRenderLayer.Default, 1, -1)]
        [TestCase(ABRenderLayer.Default, 20, -20)]
        [TestCase(ABRenderLayer.Default, 60, -60)]
        [TestCase(ABRenderLayer.AbovePlayer, 0, 61)]
        [TestCase(ABRenderLayer.AbovePlayer, 60, 1)]
        [TestCase(ABRenderLayer.Background, 0, -61)]
        [TestCase(ABRenderLayer.Background, 60, -121)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void OnlyDepth_PlacesEachBandOnItsOwnStretch(ABRenderLayer band, int depth, int expected)
        {
            var layers = Resolve(new[] { Obj(depth, band) }, ABLayerImport.OnlyDepth);
            Assert.AreEqual(expected, layers[0]);
        }

        // Independent of what the level uses: the player line is an absolute depth, so a level whose
        // shallowest object is at depth 10 must not have that object promoted onto layer 0.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void OnlyDepth_IsAbsolute_NotRelativeToWhatTheLevelUses()
        {
            var sources = new[] { Obj(10), Obj(20), Obj(30) };
            var layers = Resolve(sources, ABLayerImport.OnlyDepth);

            CollectionAssert.AreEqual(new[] { -10, -20, -30 }, layers);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void OnlyDepth_DepthOutsideTheSourceRange_IsClampedIntoIt()
        {
            var layers = Resolve(new[] { Obj(-5), Obj(999) }, ABLayerImport.OnlyDepth);
            CollectionAssert.AreEqual(new[] { 0, -60 }, layers);
        }

        #endregion

        #region The player line

        [TestCase(ABLayerImport.Auto)]
        [TestCase(ABLayerImport.OnlyDepth)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void DepthDrivenModes_PutDepthZeroInFrontOfThePlayerAndEverythingElseBehind(
            ABLayerImport mode)
        {
            var sources = Enumerable.Range(0, ABLayerMap.DepthSpan).Select(d => Obj(d)).ToArray();
            var layers = Resolve(sources, mode);

            Assert.GreaterOrEqual(layers[0], 0, "depth 0 draws in front of the player");
            for (var depth = 1; depth < layers.Length; depth++)
                Assert.LessOrEqual(layers[depth], -1, $"depth {depth} draws behind the player");
        }

        [TestCase(ABLayerImport.OnlyEditor)]
        [TestCase(ABLayerImport.DepthAndEditor)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void EditorDrivenModes_PutTheWholeDefaultBandBehindThePlayer(ABLayerImport mode)
        {
            var sources = new[] { Obj(0, editorLayer: 1), Obj(30, editorLayer: 2), Obj(60, editorLayer: 3) };
            var layers = Resolve(sources, mode);

            foreach (var layer in layers)
                Assert.LessOrEqual(layer, -1, "depth orders nothing here, so nothing is promoted past the player");
        }

        [TestCase(ABLayerImport.Auto)]
        [TestCase(ABLayerImport.OnlyDepth)]
        [TestCase(ABLayerImport.OnlyEditor)]
        [TestCase(ABLayerImport.DepthAndEditor)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void EveryMode_KeepsTheThreeBandsApart(ABLayerImport mode)
        {
            // Three editor groups rather than a realistic number of them, because DepthAndEditor
            // spends a whole 61-layer band on each and three bands of those is already 549 layers -
            // a level organised any more finely than this genuinely does run out under that mode,
            // which is what its own clamping test is for.
            var sources = new List<VgdObject>();
            foreach (var band in new[]
                     {
                         ABRenderLayer.Background,
                         ABRenderLayer.Default,
                         ABRenderLayer.AbovePlayer,
                     })
                for (var depth = 0; depth <= VgdObject.MaxDepth; depth += 12)
                for (var bin = 0; bin < 3; bin++)
                    sources.Add(Obj(depth, band, 1, bin));

            var layers = Resolve(sources, mode);

            var byBand = sources
                .Select((source, index) => (Band: ABLayerMap.ToBand(source), Layer: layers[index]))
                .GroupBy(pair => pair.Band)
                .ToDictionary(group => group.Key, group => (Low: group.Min(p => p.Layer), High: group.Max(p => p.Layer)));

            Assert.Less(byBand[ABRenderLayer.Background].High,
                byBand[ABRenderLayer.Default].Low, "background is behind all content");
            Assert.Less(byBand[ABRenderLayer.Default].High,
                byBand[ABRenderLayer.AbovePlayer].Low, "above-player is in front of all content");
        }

        #endregion

        #region OnlyEditor

        // Smaller editor layer and smaller bin are further BACK, which is the author's answer and the
        // opposite of how depth reads - so this is exactly the direction that cannot be inferred.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void OnlyEditor_OrdersByLayerThenBin_SmallerFurtherBack()
        {
            var sources = new[]
            {
                Obj(0, editorLayer: 1, editorBin: 0),
                Obj(0, editorLayer: 1, editorBin: 14),
                Obj(0, editorLayer: 2, editorBin: 0),
            };

            var layers = Resolve(sources, ABLayerImport.OnlyEditor);

            Assert.Less(layers[0], layers[1], "bin 0 is behind bin 14 of the same layer");
            Assert.Less(layers[1], layers[2], "a whole editor layer is coarser than a bin");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void OnlyEditor_IgnoresDepthEntirely()
        {
            var sources = new[]
            {
                Obj(60, editorLayer: 1, editorBin: 3),
                Obj(0, editorLayer: 1, editorBin: 3),
            };

            var layers = Resolve(sources, ABLayerImport.OnlyEditor);
            Assert.AreEqual(layers[0], layers[1]);
        }

        // An object with no editor block at all belongs with editor layer 1, not with a group of its
        // own - a level where nobody sorted anything must not arrive spread across two bands.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void OnlyEditor_MissingEditorBlock_ReadsAsTheFirstGroup()
        {
            var bare = new VgdObject { Id = "bare", Depth = 20, Editor = null };
            var sources = new[] { bare, Obj(20, editorLayer: 1, editorBin: 0) };

            var layers = Resolve(sources, ABLayerImport.OnlyEditor);
            Assert.AreEqual(layers[1], layers[0]);
        }

        #endregion

        #region DepthAndEditor

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void DepthAndEditor_GivesEachGroupItsOwnBand_DepthOrderingInsideIt()
        {
            var sources = new[]
            {
                Obj(20, editorLayer: 1, editorBin: 0),
                Obj(10, editorLayer: 1, editorBin: 0),
                Obj(60, editorLayer: 1, editorBin: 1),
            };

            var layers = Resolve(sources, ABLayerImport.DepthAndEditor);

            Assert.Less(layers[0], layers[1], "inside one group, shallower draws in front");
            Assert.Less(layers[1], layers[2],
                "the deepest object of the next group still draws in front of the shallowest of this one");
        }

        // The mode that deliberately does NOT pack: a level using two depths still costs two whole
        // bands, and the gap between them is what Auto exists to remove.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void DepthAndEditor_LeavesTheUnusedLayersEmpty()
        {
            var sources = new[]
            {
                Obj(20, editorLayer: 1, editorBin: 0),
                Obj(20, editorLayer: 1, editorBin: 1),
            };

            var layers = Resolve(sources, ABLayerImport.DepthAndEditor);
            Assert.AreEqual(ABLayerMap.DepthSpan, layers[1] - layers[0]);
        }

        #endregion

        #region Auto

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Auto_PacksTheUsedDepthsIntoConsecutiveLayers()
        {
            var sources = new[] { Obj(5), Obj(20), Obj(55) };
            var layers = Resolve(sources, ABLayerImport.Auto);

            CollectionAssert.AreEqual(new[] { -1, -2, -3 }, layers);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Auto_ObjectsSharingADepthAndAGroup_ShareALayer()
        {
            var sources = new[]
            {
                Obj(20, editorLayer: 2, editorBin: 3, id: "a"),
                Obj(20, editorLayer: 2, editorBin: 3, id: "b"),
            };

            var layers = Resolve(sources, ABLayerImport.Auto);
            Assert.AreEqual(layers[0], layers[1]);
        }

        // The "separate what overlaps" half: same depth, different editor group, so they must not be
        // stacked into one row - and the group is only ever a TIE-BREAK, never an ordering of its own.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Auto_SeparatesGroupsThatCollideOnADepth_WithoutReorderingByGroup()
        {
            var sources = new[]
            {
                Obj(20, editorLayer: 1, id: "shallow-first-group"),
                Obj(20, editorLayer: 5, id: "shallow-last-group"),
                Obj(40, editorLayer: 5, id: "deep-last-group"),
            };

            var layers = Resolve(sources, ABLayerImport.Auto);

            Assert.AreNotEqual(layers[0], layers[1], "one depth, two groups, two layers");
            Assert.Less(layers[0], layers[1], "inside one depth the later group draws in front");
            Assert.Less(layers[2], layers[0], "depth still decides across depths, group only inside one");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Auto_NeverReordersWhatOnlyDepthDrew()
        {
            var sources = new List<VgdObject>();
            for (var depth = 0; depth <= VgdObject.MaxDepth; depth += 7)
            for (var editorLayer = 1; editorLayer <= 4; editorLayer++)
                sources.Add(Obj(depth, ABRenderLayer.Default, editorLayer, editorLayer * 3));

            var depthOnly = Resolve(sources, ABLayerImport.OnlyDepth);
            var auto = Resolve(sources, ABLayerImport.Auto);

            for (var i = 0; i < sources.Count; i++)
            for (var j = 0; j < sources.Count; j++)
            {
                if (depthOnly[i] >= depthOnly[j]) continue;
                Assert.Less(auto[i], auto[j],
                    $"{sources[i].Id} drew behind {sources[j].Id} by depth and must still do so");
            }
        }

        // A level far larger than anything the corpus holds, organised as finely as the source editor
        // allows - 4392 distinct (band, depth, group) combinations, more than there are layers. This
        // is the case that exercises the fallback: the group tie-break is dropped and the level is
        // re-ranked by band and depth alone, which fits with room to spare. Clamping instead would
        // collapse the deepest 3000 objects onto one row and say so, which is the outcome the whole
        // Auto mode exists to avoid.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Auto_AFinelyOrganisedLevel_StillFitsTheAuthoredLayerRange()
        {
            var sources = new List<VgdObject>();
            foreach (var band in new[]
                     {
                         ABRenderLayer.Background,
                         ABRenderLayer.Default,
                         ABRenderLayer.AbovePlayer,
                     })
                for (var depth = 0; depth <= VgdObject.MaxDepth; depth++)
                for (var editorLayer = 1; editorLayer <= 8; editorLayer++)
                for (var bin = 0; bin <= ABLayerMap.MaxEditorBin; bin += 7)
                    sources.Add(Obj(depth, band, editorLayer, bin));

            var report = new InteropReport();
            var result = ABLayerMap.Resolve(sources, Options(ABLayerImport.Auto), report);

            Assert.GreaterOrEqual(result.Lowest, ValueRules.MinLayer);
            Assert.LessOrEqual(result.Highest, ValueRules.MaxLayer);
            Assert.IsFalse(report.Issues.Any(issue => issue.Code == "layers_clamped"),
                "packing is what keeps a level this fine inside the range");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Auto_LeavesNoGapBetweenTheLayersItUses()
        {
            var sources = new List<VgdObject>();
            foreach (var band in new[] { ABRenderLayer.Background, ABRenderLayer.Default })
                for (var depth = 0; depth <= VgdObject.MaxDepth; depth += 9)
                    sources.Add(Obj(depth, band));

            var result = ABLayerMap.Resolve(sources, Options(ABLayerImport.Auto));
            var used = result.Layers.Distinct().OrderBy(layer => layer).ToArray();

            for (var i = 1; i < used.Length; i++)
                Assert.AreEqual(1, used[i] - used[i - 1], "packed means consecutive");
        }

        #endregion

        #region Range and reporting

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void DepthAndEditor_MoreGroupsThanThereIsRoomFor_IsClampedAndReported()
        {
            var sources = Enumerable.Range(1, 200)
                .Select(editorLayer => Obj(20, ABRenderLayer.Default, editorLayer))
                .ToArray();

            var report = new InteropReport();
            var result = ABLayerMap.Resolve(sources, Options(ABLayerImport.DepthAndEditor), report);

            Assert.GreaterOrEqual(result.Lowest, ValueRules.MinLayer);
            Assert.LessOrEqual(result.Highest, ValueRules.MaxLayer);
            Assert.IsTrue(report.Issues.Any(issue => issue.Code == "layers_clamped"),
                "an author whose level lost ordering is told so");
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Resolve_AnEmptyList_AnswersAnEmptyRange()
        {
            var result = ABLayerMap.Resolve(new List<VgdObject>(), Options(ABLayerImport.Auto));

            Assert.IsEmpty(result.Layers);
            Assert.AreEqual(0, result.Lowest);
            Assert.AreEqual(0, result.Highest);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void ToBand_AnUndefinedValue_ReadsAsDefault()
        {
            Assert.AreEqual(ABRenderLayer.Default,
                ABLayerMap.ToBand(new VgdObject { RenderLayer = 7 }));
            Assert.AreEqual(ABRenderLayer.Background,
                ABLayerMap.ToBand(new VgdObject { RenderLayer = (int)ABRenderLayer.Background }));
        }

        #endregion
    }
}
