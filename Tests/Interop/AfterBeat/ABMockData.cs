using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Models;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // The shared fixture builder for this folder, same role Tests/MockData.cs plays for the format
    // itself. Two kinds of factory, and the split matters:
    //
    //   CreateXxx builds a MINIMAL valid document - one object, one keyframe - so a test asserting
    //   about one field is not reading past four others that happen to be set.
    //   CreateFullXxx touches as much field surface as it can while staying legal, which is what a
    //   round trip needs to be worth running.
    //
    // Nothing here is a real level. Real levels are somebody else's user content and live outside
    // this repository - see ABCorpusTests.
    internal static class ABMockData
    {
        public const string ThemeSourceId = "theme-a";
        public const string ObjectSourceId = "obj-1";
        public const string ChildSourceId = "obj-2";

        #region Themes

        /// <summary> A theme whose every slot is a distinct colour, so a mapping that swaps two
        /// bands cannot pass. </summary>
        public static VgtTheme CreateTheme()
        {
            var theme = new VgtTheme
            {
                Id = ThemeSourceId,
                Name = "Test Theme",
                Background = "010203",
                Gui = "040506",
                GuiAccent = "070809",
                Players = new List<string>(),
                Objects = new List<string>(),
                Effects = new List<string>(),
                Parallax = new List<string>(),
            };

            for (var i = 0; i < VgtTheme.PlayerCount; i++) theme.Players.Add(Hex(0x10 + i));
            for (var i = 0; i < VgtTheme.ObjectCount; i++) theme.Objects.Add(Hex(0x20 + i));
            for (var i = 0; i < VgtTheme.EffectCount; i++) theme.Effects.Add(Hex(0x30 + i));
            for (var i = 0; i < VgtTheme.ParallaxCount; i++) theme.Parallax.Add(Hex(0x40 + i));

            return theme;
        }

        private static string Hex(int channel) => $"{channel:X2}{channel:X2}{channel:X2}";

        #endregion

        #region Objects

        /// <summary> One square that moves once. Autokill Last Keyframe, so its lifetime is decided
        /// by the keyframe below it. </summary>
        public static VgdObject CreateObject(string id = ObjectSourceId)
        {
            var target = new VgdObject
            {
                Id = id,
                Name = "Square",
                ObjectType = (int)ABObjectType.Hit,
                StartTime = 1f,
                AutokillType = (int)ABAutokillType.LastKeyframe,
                Shape = (int)ABShape.Square,
                ShapeOption = 0,
                Depth = VgdObject.DefaultDepth,
            };

            target.Move.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 0f, 0f } });
            target.Move.Keyframes.Add(new VgdKeyframe { Time = 2f, Values = new List<float> { 5f, -5f } });
            target.Scale.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 2f, 3f } });
            target.Color.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 1f, 1f, 0f } });

            return target;
        }

        /// <summary> Two rotation keyframes, each a delta from the one before - the shape the
        /// importer has to accumulate. </summary>
        public static VgdObject CreateRotatingObject()
        {
            var target = CreateObject();
            target.Rotate.Keyframes.Add(new VgdKeyframe { Time = 0f, Values = new List<float> { 90f } });
            target.Rotate.Keyframes.Add(new VgdKeyframe { Time = 1f, Values = new List<float> { 90f } });
            return target;
        }

        #endregion

        #region Levels

        /// <summary> A level with a theme, one object, one marker and one checkpoint. </summary>
        public static VgdLevel CreateLevel()
        {
            var level = new VgdLevel();
            level.Themes.Add(CreateTheme());
            level.Objects.Add(CreateObject());

            level.Markers.Add(new VgdMarker { Id = "m1", Name = "Drop", Time = 4f });
            level.Checkpoints.Add(new VgdCheckpoint
            {
                Id = "c1",
                Name = "Halfway",
                Time = 6f,
                Position = new VgdVector2(3f, -4f),
            });

            level.Editor.Bpm.Value = 128f;
            level.Editor.Bpm.Offset = 0.25f;

            return level;
        }

        /// <summary> The level above plus a parented child, a prefab and its placement, and one
        /// keyframe on every event track that maps somewhere. </summary>
        public static VgdLevel CreateFullLevel()
        {
            var level = CreateLevel();

            var child = CreateObject(ChildSourceId);
            child.ParentId = ObjectSourceId;
            child.Depth = VgdObject.DefaultDepth - 5;
            level.Objects.Add(child);

            var prefab = new VgpPrefab { Id = "p1", Name = "Burst" };
            prefab.Objects.Add(CreateObject("inner-1"));
            level.Prefabs.Add(prefab);

            var placement = new VgdPrefabPlacement { Id = "pl1", PrefabId = "p1" };
            placement.Tracks[VgdPrefabPlacement.TrackIndex.Position].Values = new List<float> { 1f, 2f };
            placement.Tracks[VgdPrefabPlacement.TrackIndex.Scale].Values = new List<float> { 1f, 1f };
            placement.Tracks[VgdPrefabPlacement.TrackIndex.Rotation].Values = new List<float> { 0f };
            level.PrefabPlacements.Add(placement);

            level.SetEvents(ABEventTrack.CameraPosition,
                new List<VgdEventKeyframe> { Event(0f, 1f, 2f) });
            level.SetEvents(ABEventTrack.CameraZoom,
                new List<VgdEventKeyframe> { Event(0f, 12f) });
            level.SetEvents(ABEventTrack.CameraRotation,
                new List<VgdEventKeyframe> { Event(0f, 45f) });
            level.SetEvents(ABEventTrack.CameraShake,
                new List<VgdEventKeyframe> { Event(0f, 0.5f) });
            level.SetEvents(ABEventTrack.Bloom,
                new List<VgdEventKeyframe> { Event(0f, 1f, 0.5f, 2f) });
            level.SetEvents(ABEventTrack.Vignette,
                new List<VgdEventKeyframe> { Event(0f, 0.3f, 0.4f, 1f, 0f, 0.5f, 0.5f, 1f) });
            level.SetEvents(ABEventTrack.Chromatic,
                new List<VgdEventKeyframe> { Event(0f, 0.2f, 0f) });

            var themeKey = new VgdEventKeyframe { Time = 0f };
            themeKey.Values = new Newtonsoft.Json.Linq.JArray { ThemeSourceId };
            level.SetEvents(ABEventTrack.Theme, new List<VgdEventKeyframe> { themeKey });

            return level;
        }

        private static VgdEventKeyframe Event(float time, params float[] values)
            => new()
            {
                Time = time,
                Values = Newtonsoft.Json.Linq.JArray.FromObject(values),
            };

        #endregion

        #region Metadata

        public static VgmMeta CreateMeta()
        {
            var meta = new VgmMeta();
            meta.Song.Title = "Test Song";
            meta.Song.Description = "A level for tests.";
            meta.Song.Difficulty = (int)ABDifficulty.Advanced;
            meta.Creator.SteamName = "Creator";
            meta.Artist.Name = "Artist";
            meta.Artist.LinkType = (int)ABLinkType.Bandcamp;
            meta.Artist.Link = "someband";
            return meta;
        }

        #endregion
    }
}
