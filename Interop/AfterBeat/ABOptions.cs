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

        // The background is the one part of an imported level that is always on screen and never
        // authored: it spans the whole timeline, it loops by itself, and its keyframes are BAKED,
        // so it is also the part an author is most likely to want out of the way while working on
        // the content. Importing it inactive keeps the objects - the bake is expensive to redo and
        // impossible to recover once dropped - while leaving the level looking like its content
        // alone. Off by default for that reason, which is the one place in this class where the
        // default is not the one that changes the level least.

        /// <summary> Whether imported parallax objects arrive ACTIVE. Inactive ones are stored, kept
        /// and editable, and draw nothing until the author ticks them on. </summary>
        public bool ParallaxActive;

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

        // A gradient corner landing between the ramp's two ends is a BLEND of two theme colours,
        // and no theme reference expresses a blend - so a rotated or short ramp can be carried
        // either by its angle or by its theme, never by both. Neither answer is right for every
        // level, which is why this is a switch rather than a decision made in ABGradientMap.
        //
        // On is the default because reproducing how the source level LOOKS is what an importer is
        // for. Off is worth reaching for on a level built around theme switching, where a frozen
        // colour is the more visible loss of the two.

        /// <summary> Let a gradient corner become a literal colour when the ramp does not land it
        /// exactly on one of its two ends. Off snaps such a corner to its nearer end instead: a
        /// hard edge in place of a blend, with every theme reference kept alive. </summary>
        public bool BakeGradientCorners = true;

        // Afterbeat's DEPTH is what orders a level over there and it is mostly left at its default,
        // so a converted level's objects share few layers - which reads as several thousand clips
        // stacked into a handful of timeline rows. Spending draw order on the source EDITOR's own
        // grouping is what this fixed once and paid for with the range (a real level over 900
        // layers, reaching -520); the row count is the editor's problem to solve by grouping, not
        // draw order's. Auto is depth alone, packed - see ABLayerMap. The other three modes are
        // there for an author who wants the source editor's organisation expressed as layers
        // anyway, and they are the ones that can run out of range.

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
        /// it. Zero, the default, gives a placement no draw order of its own, which is what the
        /// source game does with one - see ABPrefabImporter's ResolveLayer. Raising it pulls every
        /// placement in front of the level and spreads them over a layer each, which is a timeline
        /// row per placement and a level drawn in an order Afterbeat never drew it in. </summary>
        public int PlacementLayerOffset;

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
