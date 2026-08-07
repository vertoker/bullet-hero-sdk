using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Modifiers
{
    // The one generator that only ever DELETES, and the only one for which the context window is an
    // instruction rather than a boundary. Everywhere else [StartFrame, EndFrame] says where new
    // content may be written; here it says which content is being talked about - which is what makes
    // "clean up everything the level can no longer play" (window = whole level, Invert on) and "wipe
    // this section" (window = the section, Invert off) the same operation with one flag between them.
    // A host that offers a whole-level switch on its window - the in-game editor does - gets the
    // first for free without this generator knowing anything about a level's own FrameLength.
    //
    // Partial overlap survives BOTH modes on purpose, and the asymmetry in Doomed() is what
    // implements it: Invert deletes what shares NO frame with the window, and its opposite deletes
    // only what lies WHOLLY inside it. Making both tests "overlaps" would mean an object hanging over
    // the edge is deleted whichever way the flag is set, which is not a mode - it is a trap.
    //
    // Deleting is never per-object: an object being removed may still PARENT one that is staying,
    // and dropping it alone leaves a dangling ParentObjectId that LevelGraphAnalyzer then reports -
    // a cleanup pass that breaks the level it cleaned. Hence keep-marking (a surviving object marks
    // its whole ancestor chain) instead of a per-object test, plus the same for a surviving
    // PrefabObject's materialized children, which its own ObjectIds table points at.

    /// <summary>
    /// Deletes level content by frame range: everything outside the run's window, or everything
    /// inside it. Objects always, audio tracks and level-global event keyframes on request.
    /// </summary>
    public class ContentRemoverGenerator : BaseModifier<ContentRemoverGenerator.Parameters>
    {
        public override string NameKey => "mod_content_remover";

        // Whole-scope rather than the selection: "everything in/outside this range" is a statement
        // about the scope, and quietly limiting it to what happens to be selected would leave the
        // rest behind while reporting success. Not LevelScope either, even though Audio/EventFrames
        // need Game/Audio - the window comes from the context, so removing objects is just as
        // meaningful inside a Prefab template, and declaring LevelScope would make a host disable the
        // whole generator there for the sake of two switches that default to off.

        /// <summary> Nothing beyond a scope to run against. </summary>
        public override GeneratorRequirements Requirements => GeneratorRequirements.None;

        // One section for four switches: splitting them would put a header above each half of a form
        // short enough to read at a glance, and a host renders no header at all while there is only
        // one section (Section is still what every field is listed through - see
        // HintsSections_CoverEveryParameterField).
        public override GeneratorHints Hints { get; } = new GeneratorHints.Builder()
            .Section(GeneratorSections.Main, nameof(Parameters.Invert), nameof(Parameters.Objects),
                nameof(Parameters.Audio), nameof(Parameters.EventFrames))
            .Build();

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var start = context.StartFrame;
            var end = context.EndFrame;
            if (end < start) return;

            if (parameters.Objects) RemoveObjects(context, start, end, parameters.Invert);
            if (parameters.Audio) RemoveAudioTracks(context, start, end, parameters.Invert);
            if (parameters.EventFrames) RemoveEventKeys(context, start, end, parameters.Invert);
        }

        // Deletes only. GeneratorCost describes what a run ADDS, and reporting anything here would
        // read as "this will add N", which is the opposite of what happens.
        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
            => GeneratorCost.Zero;

        // The two configurations whose reach is larger than the window the author is looking at:
        // Invert deletes everything the window does NOT cover (so the smaller the window, the more
        // it takes), and a window spanning the whole timeline covers all of it either way. Both are
        // legitimate - they are the generator's two headline use cases - which is exactly why they
        // get a confirmation instead of a refusal.
        protected override bool IsDangerousTyped(GeneratorContext context, Parameters parameters)
            => parameters.Invert || CoversWholeTimeline(context);

        // The active timeline, not always the level's: inside Prefab Mode the window is bounded by
        // the template's own FrameLength (Prefab implements IFrameLength, Level.Game doesn't), which
        // is the same rule the editor's own window clamp uses.
        private static bool CoversWholeTimeline(GeneratorContext context)
        {
            if (context == null) return false;

            var frameLength = context.Scope is IFrameLength scope
                ? scope.FrameLength
                : context.Settings?.FrameLength ?? 0;

            return context.StartFrame <= FrameRules.MinFrame && context.EndFrame >= frameLength - 1;
        }

        private static void RemoveObjects(GeneratorContext context, int start, int end, bool invert)
        {
            var keep = new HashSet<ObjectId>();
            foreach (var pair in context.Objects)
                if (!Doomed(pair.Value.StartFrame, pair.Value.EndFrame, start, end, invert))
                    KeepWithAncestors(context, keep, pair.Key);

            KeepMaterializedChildren(context, keep);

            // Collected first: Delete writes through the very dictionary being enumerated.
            var doomed = new List<ObjectId>();
            foreach (var id in context.Objects.Keys)
                if (!keep.Contains(id))
                    doomed.Add(id);

            foreach (var id in doomed) context.Delete(id);
        }

        private static void RemoveAudioTracks(GeneratorContext context, int start, int end, bool invert)
        {
            // Null in Prefab Mode - a template has objects but no scheduled audio, so there is
            // nothing to remove rather than something to refuse.
            if (context.Audio?.Tracks == null) return;

            var doomed = new List<AudioId>();
            foreach (var pair in context.Audio.Tracks)
                if (Doomed(pair.Value.StartFrame, pair.Value.EndFrame, start, end, invert))
                    doomed.Add(pair.Key);

            foreach (var id in doomed) context.RemoveResource(context.Audio.Tracks, id);
        }

        /// <summary> Every level-global track, all 25 of them. They carry no owning object, so they
        /// go through RemoveLevelKeys rather than riding along on an Edit. Null in Prefab Mode, same
        /// as audio. </summary>
        private static void RemoveEventKeys(GeneratorContext context, int start, int end, bool invert)
        {
            if (context.Game == null) return;

            var events = context.Game.Events;
            Trim(context, events.Markers, start, end, invert);
            Trim(context, events.Checkpoints, start, end, invert);
            Trim(context, events.ScreenLimits, start, end, invert);
            Trim(context, events.Backgrounds, start, end, invert);
            Trim(context, events.Themes, start, end, invert);

            var camera = context.Game.CameraEvents;
            Trim(context, camera.Positions, start, end, invert);
            Trim(context, camera.Rotations, start, end, invert);
            Trim(context, camera.Zooms, start, end, invert);
            Trim(context, camera.Pivots, start, end, invert);
            Trim(context, camera.Shakes, start, end, invert);

            var post = context.Game.PostProcessingEvents;
            Trim(context, post.Blooms, start, end, invert);
            Trim(context, post.Chromatics, start, end, invert);
            Trim(context, post.Vignettes, start, end, invert);
            Trim(context, post.Lenses, start, end, invert);
            Trim(context, post.Grains, start, end, invert);
            Trim(context, post.MotionBlurs, start, end, invert);
            Trim(context, post.ColorCurveses, start, end, invert);
            Trim(context, post.LiftGammaGains, start, end, invert);
            Trim(context, post.ShadowsMidtonesHighlightses, start, end, invert);
            Trim(context, post.WhiteBalances, start, end, invert);
            Trim(context, post.AnalogGlitches, start, end, invert);
            Trim(context, post.DigitalGlitches, start, end, invert);

            var player = context.Game.PlayerEvents;
            Trim(context, player.Visibles, start, end, invert);
            Trim(context, player.Controls, start, end, invert);
            Trim(context, player.Collisions, start, end, invert);
        }

        /// <summary> A keyframe is a point, so "overlaps the window" and "lies inside it" are the same
        /// question - the two modes are exact opposites here, with no partial case in between. </summary>
        private static void Trim<TKey>(GeneratorContext context, List<TKey> track, int start, int end, bool invert)
            where TKey : IFrame
        {
            if (track == null) return;
            context.RemoveLevelKeys(track, key => invert
                ? key.Frame < start || key.Frame > end
                : key.Frame >= start && key.Frame <= end);
        }

        /// <summary> Marks a surviving object and everything it hangs off, so a parent is never
        /// deleted out from under a child that stays. Depth-guarded: this walks author data, and a
        /// cyclic parent chain must not hang the run. </summary>
        private static void KeepWithAncestors(GeneratorContext context, HashSet<ObjectId> keep, ObjectId id)
        {
            for (var guard = 0; guard < LevelRules.MaxObjectDepth; guard++)
            {
                if (!context.Objects.TryGetValue(id, out var obj)) return;

                // Already marked means its own chain was walked on an earlier object's behalf.
                if (!keep.Add(id)) return;

                id = obj.ParentObjectId;
                if (!id.IsNotNull()) return;
            }
        }

        // A materialized child is an ordinary object in every other respect, but its placement's
        // ObjectIds table points straight at it - deleting one while the placement survives is a
        // broken remap table, which is a validation error rather than a tidier level. A placement
        // that is itself doomed is not kept in the first place, and then neither are its children,
        // so this only ever rescues the mixed case.

        /// <summary> Extends the keep set through every surviving PrefabObject's own materialized
        /// children, nested placements included. </summary>
        private static void KeepMaterializedChildren(GeneratorContext context, HashSet<ObjectId> keep)
        {
            var pending = new Stack<ObjectId>(keep);
            while (pending.Count > 0)
            {
                var id = pending.Pop();
                if (!context.Objects.TryGetValue(id, out var obj)) continue;
                if (obj is not PrefabObject placement || placement.ObjectIds == null) continue;

                foreach (var child in placement.ObjectIds.Values)
                    if (context.Objects.ContainsKey(child) && keep.Add(child))
                        pending.Push(child);
            }
        }

        /// <summary> Whether a [objectStart, objectEnd] lifetime is what this run removes: with Invert,
        /// one sharing no frame at all with the window; without it, one lying wholly inside. </summary>
        private static bool Doomed(int objectStart, int objectEnd, int start, int end, bool invert)
            => invert
                ? objectStart > end || objectEnd < start
                : objectStart >= start && objectEnd <= end;

        public class Parameters
        {
            /// <summary> On: remove what falls outside the frame range. Off (the default, and the
            /// narrower of the two): remove what falls inside it. Content only partly overlapping the
            /// range survives either way. </summary>
            public bool Invert;

            /// <summary> Remove objects. </summary>
            public bool Objects = true;

            /// <summary> Remove audio tracks. Level scope only - a Prefab template has no audio. </summary>
            public bool Audio;

            /// <summary> Remove level-global event keyframes (camera, post-processing, player,
            /// markers, checkpoints, ...). Level scope only, same as Audio. </summary>
            public bool EventFrames;
        }
    }
}
