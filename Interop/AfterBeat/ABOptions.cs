using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    /// <summary>
    /// The handful of choices a conversion cannot make on its own. Every default here is the one
    /// that changes the level least.
    /// </summary>
    public class ABOptions
    {
        /// <summary> What a fresh <see cref="Models.SettingGroups.LevelSettings"/> runs at.
        /// FrameRules has no constant for it - the number lives in that class's own constructor. </summary>
        public const int DefaultFramerate = 60;

        /// <summary> Frames per second the imported level runs at. Afterbeat stores seconds, so
        /// this decides how finely every time in the file is resolved. </summary>
        public int Framerate = DefaultFramerate;

        /// <summary> Turn the five parallax layers into ordinary collider-less objects. Off means
        /// the level's whole background is dropped. </summary>
        public bool ImportParallax = true;

        /// <summary> Read prefabs and their placements. Off flattens nothing - the placements are
        /// simply not created, and the templates not stored. </summary>
        public bool ImportPrefabs = true;

        /// <summary> How many keyframes a baked parallax loop may spend. Capped by the format's own
        /// per-track limit; more cycles than this and the loop is truncated rather than thinned,
        /// since a thinned loop drifts out of phase with the rest of the background. </summary>
        public int MaxParallaxLoopKeys = LevelRules.MaxObjectKeys;

        /// <summary> Name imported objects after their Afterbeat name, falling back to their source
        /// id. Off leaves every name empty, which is what an author gets from this editor's own
        /// create. </summary>
        public bool KeepObjectNames = true;

        // Everything in a converted level used to land on layer 0, because Afterbeat's DEPTH is
        // mostly left at its default and its editor layers - the thing that actually organises a
        // level over there - are bookkeeping this format has no field for. The result was a
        // timeline of several thousand clips stacked in one row. Spending a fixed band per editor
        // group fixed the row count and broke the range instead, which is what Auto exists for.

        /// <summary> What the converted level's draw order is derived from. See
        /// <see cref="ABLayerImport"/> - this changes what draws in front of what, so it is
        /// the one import choice worth showing an author first. </summary>
        public ABLayerImport LayerImport = ABLayerImport.Auto;

        /// <summary> How many layers one source editor group is given, under
        /// <see cref="ABLayerImport.DepthAndEditor"/> alone. The source format's whole depth
        /// range, so a group's own depths cannot reach into the next group's band; narrower
        /// interleaves the groups, which is a thing an author may want and this format can express. </summary>
        public int EditorGroupStride = ABLayerMap.DepthSpan;

        /// <summary> How far below the level's lowest content layer the first parallax object sits;
        /// every object behind it steps one further down. Relative rather than absolute because how
        /// deep the content itself reaches depends on the level and on
        /// <see cref="LayerImport"/> - a fixed base used to land in the middle of it. </summary>
        public int ParallaxLayerOffset = 1;

        /// <summary> How far above the level's highest content layer the first prefab placement
        /// sits; each further placement steps one up, carrying its whole materialized subtree with
        /// it. Above ordinary content, since a placement's own objects are usually the level's
        /// foreground. </summary>
        public int PlacementLayerOffset = 1;

        // THE SONG IS THE LEVEL over there. Afterbeat stores no length of its own: its timeline is
        // its audio clip, an object timed past the end of the song simply never plays, and its
        // editor cannot scroll past it either. So a converted level whose length was MEASURED off
        // its content is a level of the wrong length by definition - it ends wherever the last
        // object happened to, which is neither where the song ends nor where the author was working.
        //
        // The SDK opens no files, so it cannot measure a clip; the host that opened the folder can,
        // and fills this in. Zero means it could not, and the length falls back to being measured.

        /// <summary> Length of the level's song in SECONDS, as the host measured it. Zero when it
        /// is unknown, which is the only case where the level's length is derived from its content
        /// instead. </summary>
        public float AudioLengthSeconds;

        public ABOptions() { }

        public ABOptions(int framerate)
        {
            Framerate = framerate;
        }

        /// <summary> A copy with the framerate clamped into what the format allows - a generator's
        /// form can hand over anything. </summary>
        public ABOptions Sanitized()
        {
            var copy = (ABOptions)MemberwiseClone();
            copy.Framerate = System.Math.Clamp(Framerate, FrameRules.MinFramerate, FrameRules.MaxFramerate);
            copy.MaxParallaxLoopKeys = System.Math.Clamp(MaxParallaxLoopKeys, 2, LevelRules.MaxObjectKeys);
            copy.AudioLengthSeconds = AudioLengthSeconds > 0f ? AudioLengthSeconds : 0f;
            copy.EditorGroupStride = System.Math.Clamp(EditorGroupStride, 1, ValueRules.MaxLayer);
            copy.ParallaxLayerOffset = System.Math.Clamp(ParallaxLayerOffset, 0, ValueRules.MaxLayer);
            copy.PlacementLayerOffset = System.Math.Clamp(PlacementLayerOffset, 0, ValueRules.MaxLayer);
            return copy;
        }
    }
}
