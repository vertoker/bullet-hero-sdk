using System.Collections.Generic;
using System.Linq;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // THE CONVERTER USED TO LIVE ON A FALLBACK THAT NO LONGER SAYS WHAT IT SAID. An empty Sizes track
    // reads as ZERO here now (BH.Shared.defaults.size) - a group node with no extent of its own -
    // while over there every object is one unit square at scale 1 and there is no such thing as a
    // node without an extent. So the two directions each need a value written out where nothing used
    // to be written at all, and neither failure would have been loud: an import would simply produce
    // a level of invisible shapes, and an export a file whose group nodes became visible squares.
    //
    // The import side is the one that would have bitten, because the path it takes is the ORDINARY
    // one rather than an edge: an object whose children inherit its scale puts that scale in Scales
    // and has nothing left for its own extent, and that is 39% of the parents in the measured corpus
    // (AFTERBEAT_ISSUE.md).

    /// <summary> What each direction writes where the engine's own fallback used to answer. </summary>
    public class ABSizeSeedTests
    {
        private const int Framerate = 60;

        private static ABOptions Options() => new(Framerate);

        /// <summary> An ordinary drawn object. `parentType` "111" is what makes a child inherit its
        /// parent's scale, which is what sends the PARENT's scale to Scales. </summary>
        private static VgdObject Object(string id, string parentId = null, bool withScale = true)
        {
            var target = new VgdObject
            {
                Id = id,
                ParentId = parentId ?? string.Empty,
                ParentType = "111",
                ObjectType = (int)ABObjectType.Normal,
                Shape = (int)ABShape.Square,
                AutokillType = (int)ABAutokillType.FixedTime,
                AutokillOffset = 4f,
            };

            target.Move.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 0f, 0f } });
            target.Color.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 0f, 100f } });

            if (withScale)
                target.Scale.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 4f, 5f } });

            return target;
        }

        private static Level Import(params VgdObject[] objects)
        {
            var level = new VgdLevel();
            level.Themes.Add(ABMockData.CreateTheme());
            foreach (var source in objects) level.Objects.Add(source);

            return ABLevelImporter.Import(level, null, Options()).Level;
        }

        private static Vector2Value SizeOf(RectObject obj) => (Vector2Value)obj.Sizes.Single().Scale;

        #region Import

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AParentWhoseChildrenInheritItsScale_StillGetsAnExtentOfItsOwn()
        {
            var level = Import(Object("parent"), Object("child", "parent"));

            var parent = level.Game.Objects.Values
                .OfType<ShapeObject>()
                .Single(o => !o.ParentObjectId.IsNotNull());

            Assert.IsNotEmpty(parent.Scales, "its scale propagates, so that is where the track went");

            var size = SizeOf(parent);
            Assert.AreEqual(ValueRules.DefaultScaX, size.X, 1e-4f);
            Assert.AreEqual(ValueRules.DefaultScaY, size.Y, 1e-4f);
            Assert.AreEqual(FrameRules.MinFrame, parent.Sizes.Single().Frame);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AnObjectWithNoScaleTrackAtAll_IsStillOneUnitSquare()
        {
            var level = Import(Object("lonely", withScale: false));
            var shape = level.Game.Objects.Values.OfType<ShapeObject>().Single();

            var size = SizeOf(shape);
            Assert.AreEqual(ValueRules.DefaultScaX, size.X, 1e-4f);
            Assert.AreEqual(ValueRules.DefaultScaY, size.Y, 1e-4f);
        }

        // The ordinary case must be untouched by the seed: a childless object's scale IS its size,
        // and writing a square beside it would double the track.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Import_AChildlessObjectsScale_IsStillItsOnlySizeKeyframe()
        {
            var level = Import(Object("lonely"));
            var shape = level.Game.Objects.Values.OfType<ShapeObject>().Single();

            var size = SizeOf(shape);
            Assert.AreEqual(4f, size.X, 1e-4f);
            Assert.AreEqual(5f, size.Y, 1e-4f);
            Assert.IsEmpty(shape.Scales);
        }

        #endregion

        #region Export

        private static VgdObject Export(RectObject obj)
        {
            var level = new Level();
            obj.ObjectId = new ObjectId(1);
            obj.Span = new FrameSpan(FrameRules.MinFrame, 60);
            obj.Active = true;
            level.Game.Objects[obj.ObjectId] = obj;

            return ABLevelExporter.Export(level, null, Options()).Level?.Objects?.FirstOrDefault();
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AGroupNodeWithNoSize_SaysZeroRatherThanNothing()
        {
            var exported = Export(new RectObject { Name = "Group" });
            var track = exported.GetTrack(VgdObject.TrackIndex.Scale);

            Assert.AreEqual(1, track.Keyframes.Count, "silence would read as the far side's own one");
            Assert.AreEqual(0f, track.Keyframes[0].GetValue(0), 1e-4f);
            Assert.AreEqual(0f, track.Keyframes[0].GetValue(1), 1e-4f);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_AnObjectWithASize_IsLeftExactlyAsItIs()
        {
            var obj = new RectObject { Name = "Sized" };
            obj.Sizes.Add(new Models.Keyframes.ScaKey(new Vector2Value(4f, 5f), FrameRules.MinFrame));

            var track = Export(obj).GetTrack(VgdObject.TrackIndex.Scale);

            Assert.AreEqual(1, track.Keyframes.Count);
            Assert.AreEqual(4f, track.Keyframes[0].GetValue(0), 1e-4f);
            Assert.AreEqual(5f, track.Keyframes[0].GetValue(1), 1e-4f);
        }

        // A placement is exported from Scales, not from Sizes - its scale is the multiplier its whole
        // subtree sits inside, which is what a parent's scale track is over there. That track's
        // empty-track value is still one, so it must NOT be given a zero.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Normal)]
        public void Export_APlacementWithNoScale_IsLeftSilent()
        {
            var track = Export(new PrefabObject { Name = "Placement" })
                .GetTrack(VgdObject.TrackIndex.Scale);

            Assert.AreEqual(0, track.Keyframes.Count);
        }

        #endregion
    }
}
