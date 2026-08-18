using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    /// <summary>
    /// The handful of choices a conversion cannot make on its own. Every default here is the one
    /// that changes the level least.
    /// </summary>
    public class AfterBeatOptions
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

        /// <summary> Layer given to the frontmost parallax layer; every layer behind it steps
        /// further down. Below authored content, which is what a background is. </summary>
        public int ParallaxBaseLayer = -100;

        public AfterBeatOptions() { }

        public AfterBeatOptions(int framerate)
        {
            Framerate = framerate;
        }

        /// <summary> A copy with the framerate clamped into what the format allows - a generator's
        /// form can hand over anything. </summary>
        public AfterBeatOptions Sanitized()
        {
            var copy = (AfterBeatOptions)MemberwiseClone();
            copy.Framerate = System.Math.Clamp(Framerate, FrameRules.MinFramerate, FrameRules.MaxFramerate);
            copy.MaxParallaxLoopKeys = System.Math.Clamp(MaxParallaxLoopKeys, 2, LevelRules.MaxObjectKeys);
            copy.ParallaxBaseLayer = System.Math.Clamp(ParallaxBaseLayer, ValueRules.MinLayer, ValueRules.MaxLayer);
            return copy;
        }
    }
}
