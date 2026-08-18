using System;
using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    // Draw order, all of it, in one place: this is the only thing in the converter that cannot be
    // decided one object at a time, because two of the four modes need to see the whole list before
    // any single object's layer is known.
    //
    // THE PLAYER LINE is what the whole layout is anchored on. Afterbeat draws its player at a
    // fixed point between depth 0 and depth 1, so an ordinary object with a depth of 20 is BEHIND
    // the player and only a depth of 0 is in front of it. This format draws its own avatar at layer
    // -0.5 (Services.Shared's AvatarInitData.BaseLayer), i.e. between layer -1 and layer 0. So the
    // two lines are made to coincide: layer >= 0 is in front of the player, layer <= -1 is behind
    // it, and every mode lays its bands out around that boundary. The previous mapping put depth 20
    // on layer 0, which drew the whole of an ordinary level in FRONT of the player.
    //
    // THE THREE BANDS come from the source object's own rl - Background behind everything, Default
    // with the level's content, AbovePlayer in front of all of it. They are laid out as three
    // consecutive stretches of the same width rather than interleaved, because that is what the
    // source game does: they are separate cameras over there, and no depth in one can reach into
    // another.
    //
    // Only OnlyDepth and Auto honour the player line per object, and that is not an oversight: the
    // line is a statement about DEPTH, and in the two modes where the editor's own grouping decides
    // draw order, depth no longer orders anything. There the whole Default band sits behind the
    // player, which is where all but one depth of an ordinary level sat anyway.

    /// <summary> One .vgd object list's draw order, resolved into this format's effective (absolute,
    /// pre-parent-subtraction) layers. </summary>
    public static class ABLayerMap
    {
        /// <summary> How many depths the source format has - 0 through 60, both included. </summary>
        public const int DepthSpan = VgdObject.MaxDepth - VgdObject.MinDepth + 1;

        /// <summary> Lowest editor bin the source editor allows. </summary>
        public const int MinEditorBin = 0;

        /// <summary> Highest editor bin the source editor allows. </summary>
        public const int MaxEditorBin = 14;

        /// <summary> How many bins one source editor layer holds - 0 through 14, both included. </summary>
        public const int EditorBinSpan = MaxEditorBin - MinEditorBin + 1;

        /// <summary> Draw order of the three bands, back to front. Not the enum's own numbering -
        /// that one is the source format's and has Background last. </summary>
        public static int ToBandRank(ABRenderLayer band) => band switch
        {
            ABRenderLayer.Background => 0,
            ABRenderLayer.AbovePlayer => 2,
            _ => 1,
        };

        /// <summary> An object's band, reading anything the source format does not define as
        /// Default - which is what it writes when the author never touched the setting. </summary>
        public static ABRenderLayer ToBand(VgdObject source)
        {
            var value = source?.RenderLayer ?? 0;
            return value is (int)ABRenderLayer.AbovePlayer or (int)ABRenderLayer.Background
                ? (ABRenderLayer)value
                : ABRenderLayer.Default;
        }

        /// <summary> One object's place in the source editor's own ordering, bins inside layers.
        /// Larger is drawn in front. An object carrying no editor block reads as layer 1 - the same
        /// group as the first one, not a group of its own: an object nobody sorted belongs with the
        /// ones nobody sorted either. </summary>
        public static int ToEditorIndex(VgdObject source)
        {
            var editorLayer = Math.Max(1, source?.Editor?.Layer ?? 0);
            var bin = Math.Clamp(source?.Editor?.Bin ?? 0, MinEditorBin, MaxEditorBin);
            return (editorLayer - 1) * EditorBinSpan + bin;
        }

        /// <summary> One object's depth, clamped into what the source format allows. </summary>
        public static int ToDepth(VgdObject source)
            => Math.Clamp(source?.Depth ?? VgdObject.DefaultDepth, VgdObject.MinDepth, VgdObject.MaxDepth);

        /// <summary> What one resolve produced: a layer per object, plus the range they occupy,
        /// which is what the background and the prefab placements are placed around. </summary>
        public readonly struct Result
        {
            /// <summary> One layer per source object, in the order they were handed over. </summary>
            public int[] Layers { get; }

            /// <summary> The same, by the source object's own id. Objects with no id are absent. </summary>
            public Dictionary<string, int> ById { get; }

            /// <summary> Lowest and highest layer anything landed on; both 0 for an empty list. </summary>
            public int Lowest { get; }
            public int Highest { get; }

            public Result(int[] layers, Dictionary<string, int> byId, int lowest, int highest)
            {
                Layers = layers;
                ById = byId;
                Lowest = lowest;
                Highest = highest;
            }

            /// <summary> The layer of one source object, or 0 for one this resolve never saw. </summary>
            public int Get(string sourceId)
                => sourceId != null && ById != null && ById.TryGetValue(sourceId, out var layer) ? layer : 0;
        }

        /// <summary> Resolves a whole object list. </summary>
        public static Result Resolve(IReadOnlyList<VgdObject> sources, ABOptions options,
            InteropReport report = null, string path = null)
        {
            options ??= new ABOptions();

            var count = sources?.Count ?? 0;
            var layers = new int[count];
            if (count == 0) return new Result(layers, new Dictionary<string, int>(), 0, 0);

            if (options.LayerImport == ABLayerImport.Auto) ResolveAuto(sources, layers);
            else ResolveBanded(sources, options, layers);

            return Finish(sources, layers, report, path);
        }

        #region Banded

        // Each band is one stretch `span` layers wide, and the three sit back to back. `span` is the
        // source format's whole depth range for OnlyDepth - fixed, because the player line is a
        // statement about an absolute depth and a band sized to what the level happens to use would
        // move depth 0 off layer 0 - and the widest order the level actually reaches for the two
        // editor-driven modes, whose ordering has no fixed extent to be measured against.
        private static void ResolveBanded(IReadOnlyList<VgdObject> sources, ABOptions options,
            int[] layers)
        {
            var mode = options.LayerImport;
            var stride = Math.Max(1, options.EditorGroupStride);

            var span = DepthSpan;
            if (mode != ABLayerImport.OnlyDepth)
            {
                var widest = 0;
                foreach (var source in sources)
                {
                    if (source == null) continue;
                    widest = Math.Max(widest, ToOrder(source, mode, stride));
                }

                span = widest + 1;
            }

            // How many of the Default band's own orders sit in FRONT of the player: the single
            // depth-0 one where depth is what orders, none where it is not.
            var aboveCut = mode == ABLayerImport.OnlyDepth ? 1 : 0;
            var defaultBase = aboveCut - span;

            for (var i = 0; i < layers.Length; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                var bandBase = defaultBase + (ToBandRank(ToBand(source)) - 1) * span;
                layers[i] = bandBase + ToOrder(source, mode, stride);
            }
        }

        private static int ToOrder(VgdObject source, ABLayerImport mode, int stride) => mode switch
        {
            ABLayerImport.OnlyDepth => VgdObject.MaxDepth - ToDepth(source),
            ABLayerImport.OnlyEditor => ToEditorIndex(source),
            ABLayerImport.DepthAndEditor
                => ToEditorIndex(source) * stride + VgdObject.MaxDepth - ToDepth(source),
            _ => VgdObject.MaxDepth - ToDepth(source),
        };

        #endregion

        #region Auto

        // Two things at once, and they are the same operation: every DISTINCT ordering key the level
        // actually uses gets its own layer (so two objects the source editor kept apart are not
        // stacked into one row), and those layers are consecutive (so a level using six depths costs
        // six rows rather than the sixty-one its depth range spans). Rank, in other words - which is
        // also why it cannot be computed per object.
        //
        // The tie-break is the editor grouping, UNDER the depth: Auto is OnlyDepth with the
        // collisions resolved, not a third ordering. If a level is so finely divided that the ranks
        // do not fit in the authored layer range, the tie-break is dropped and the whole thing
        // re-ranked - a level drawn slightly flatter than its author organised it beats one whose
        // deepest content is clamped into a single row.
        private static void ResolveAuto(IReadOnlyList<VgdObject> sources, int[] layers)
        {
            if (TryRank(sources, layers, true)) return;
            TryRank(sources, layers, false);
        }

        private static bool TryRank(IReadOnlyList<VgdObject> sources, int[] layers, bool separateGroups)
        {
            var keys = new long[layers.Length];
            var distinct = new SortedSet<long>();

            for (var i = 0; i < layers.Length; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                keys[i] = ToKey(source, separateGroups);
                distinct.Add(keys[i]);
            }

            var ranks = new Dictionary<long, int>(distinct.Count);
            var pivot = 0;
            var rank = 0;

            foreach (var key in distinct)
            {
                ranks[key] = rank++;
                if (IsBehindPlayer(key)) pivot = rank;
            }

            if (pivot > -ValueRules.MinLayer || distinct.Count - pivot - 1 > ValueRules.MaxLayer) return false;

            for (var i = 0; i < layers.Length; i++)
            {
                if (sources[i] == null) continue;
                layers[i] = ranks[keys[i]] - pivot;
            }

            return true;
        }

        // One sortable number per ordering key rather than a comparer over a struct: the ranking is
        // a set operation, and a long that sorts back-to-front by construction makes it one.
        private static long ToKey(VgdObject source, bool separateGroups)
        {
            var band = (long)ToBandRank(ToBand(source));
            var order = (long)(VgdObject.MaxDepth - ToDepth(source));
            var group = separateGroups ? (long)ToEditorIndex(source) : 0L;

            return (band << 48) | (order << 32) | (uint)Math.Min(group, uint.MaxValue);
        }

        // Everything in the Background band, and everything in the Default band except the single
        // depth-0 order - see the player line in this file's header.
        private static bool IsBehindPlayer(long key)
        {
            var band = (int)(key >> 48);
            var order = (int)((key >> 32) & 0xFFFF);

            if (band < ToBandRank(ABRenderLayer.Default)) return true;
            return band == ToBandRank(ABRenderLayer.Default) && order < VgdObject.MaxDepth;
        }

        #endregion

        #region Finish

        private static Result Finish(IReadOnlyList<VgdObject> sources, int[] layers,
            InteropReport report, string path)
        {
            var byId = new Dictionary<string, int>(layers.Length);
            var lowest = int.MaxValue;
            var highest = int.MinValue;
            var clamped = false;

            for (var i = 0; i < layers.Length; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                var layer = Math.Clamp(layers[i], ValueRules.MinLayer, ValueRules.MaxLayer);
                if (layer != layers[i]) clamped = true;

                layers[i] = layer;
                if (!string.IsNullOrEmpty(source.Id)) byId[source.Id] = layer;

                if (layer < lowest) lowest = layer;
                if (layer > highest) highest = layer;
            }

            if (clamped)
                report?.Approximated("layers_clamped",
                    "This level is organised more finely than there is draw order to spend on it, so the outermost objects share a layer. The Auto layer mode packs the same level into far fewer layers.",
                    path);

            if (lowest > highest) return new Result(layers, byId, 0, 0);
            return new Result(layers, byId, lowest, highest);
        }

        #endregion
    }
}
