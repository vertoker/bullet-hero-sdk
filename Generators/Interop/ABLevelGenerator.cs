using System.Collections.Generic;
using BH.SDK.Generators.External;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Enums.Resources;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Interop
{
    // A level generator rather than a menu command, because "build a whole Level and LevelMeta from
    // a few parameters" is exactly what the generator contract already describes, and because it
    // then costs the host no UI: the form, the presets, the estimate and the localization all come
    // off the contract. Importing a foreign level is authoring automation like any other.
    //
    // The reverse direction is deliberately NOT a generator. A generator produces content; an
    // export consumes a level and writes files, which is a host service (see
    // Interop/AfterBeat/ABInterop.ExportLevel).
    //
    // ExternalAnalysis because the host is what probes an Afterbeat folder: which files it holds is
    // undocumented and differs per level, so the host opens it and fills in IABLevelInput before
    // this runs. Handed nothing, this produces an empty level and says why - never a
    // plausible-looking one. (The SDK CAN read a file now - see Services/Content - and the level
    // package import goes through the same interface for the different reason its own header gives:
    // a passphrase and an open handle have no business in a serialized preset.)

    /// <summary> Builds a level out of an Afterbeat (Project Arrhythmia) level folder. </summary>
    public class ABLevelGenerator : BaseLevelGenerator<ABLevelGenerator.Parameters>
    {
        public override string NameKey => "gen_level_afterbeat";

        // Last: an import from another game's format is nobody's first answer to "make a level".
        public override int ListOrder => 10;

        public override GeneratorRequirements Requirements => GeneratorRequirements.ExternalAnalysis;

        public override GeneratorHints Hints => HintsValue;

        private static readonly GeneratorHints HintsValue = new GeneratorHints.Builder()
            .Section(GeneratorSections.Main, nameof(Parameters.Framerate),
                nameof(Parameters.ImportParallax), nameof(Parameters.ImportPrefabs),
                nameof(Parameters.LayerImport))
            // The host-filled fields are listed like any other - Hidden decides whether a row is
            // SHOWN, not whether the field is accounted for, and a field in no section still
            // renders, at the bottom, where nobody would look for it.
            .Section(GeneratorSections.Additional, nameof(Parameters.KeepObjectNames),
                nameof(Parameters.OpacityHitThreshold),
                nameof(Parameters.EditorGroupStride), nameof(Parameters.PlacementLayerOffset),
                nameof(Parameters.ParallaxActive), nameof(Parameters.ParallaxLayerOffset),
                nameof(Parameters.MaxParallaxLoopKeys),
                nameof(Parameters.LevelJson), nameof(Parameters.MetaJson),
                nameof(Parameters.AudioFileName), nameof(Parameters.SourceFolder),
                nameof(Parameters.AudioLengthSeconds))
            .Range(nameof(Parameters.Framerate), FrameRules.MinFramerate, FrameRules.MaxFramerate)
            .Range(nameof(Parameters.ParallaxLayerOffset), 0, ValueRules.MaxLayer)
            .Range(nameof(Parameters.MaxParallaxLoopKeys), 2, LevelRules.MaxObjectKeys)
            .Range(nameof(Parameters.EditorGroupStride), 1, ValueRules.MaxLayer)
            .Range(nameof(Parameters.PlacementLayerOffset), 0, ValueRules.MaxLayer)
            // Zero is "the host could not measure the song"; the top is the longest level this
            // format can hold at all, which is what a range on a host-filled field is for - not a
            // slider an author drags, but a bound a hostile value cannot get past.
            .Range(nameof(Parameters.AudioLengthSeconds), 0f,
                FrameRules.MaxFrameDuration / (float)FrameRules.MinFramerate)
            // Zero is not "hit at any alpha" but "leave every collider alone" - see
            // ABOptions.OpacityHitThreshold. The range is the whole alpha range because the rule
            // being relaxed is stated in alpha, and any value between the two ends is a level that
            // hits for more of its fades than the source game did.
            .Range(nameof(Parameters.OpacityHitThreshold), 0f, ABOptions.DefaultOpacityHitThreshold)
            .Unit(nameof(Parameters.Framerate), "fps")
            .Unit(nameof(Parameters.OpacityHitThreshold), "alpha")
            .Unit(nameof(Parameters.MaxParallaxLoopKeys), "keys")
            .Unit(nameof(Parameters.EditorGroupStride), "layers")
            .Unit(nameof(Parameters.ParallaxLayerOffset), "layers")
            .Unit(nameof(Parameters.PlacementLayerOffset), "layers")
            // The one band width nothing derives - every other mode packs or spans on its own.
            .VisibleWhen(nameof(Parameters.EditorGroupStride),
                p => ((Parameters)p).LayerImport == ABLayerImport.DepthAndEditor)
            .VisibleWhen(nameof(Parameters.PlacementLayerOffset),
                p => ((Parameters)p).ImportPrefabs)
            .VisibleWhen(nameof(Parameters.ParallaxLayerOffset),
                p => ((Parameters)p).ImportParallax)
            .VisibleWhen(nameof(Parameters.ParallaxActive),
                p => ((Parameters)p).ImportParallax)
            // The host fills these in from the folder it opened; showing them as editable rows would
            // invite an author to paste a level document into a text field.
            .Unit(nameof(Parameters.AudioLengthSeconds), "s")
            .Hidden(nameof(Parameters.LevelJson))
            .Hidden(nameof(Parameters.MetaJson))
            .Hidden(nameof(Parameters.AudioFileName))
            .Hidden(nameof(Parameters.SourceFolder))
            .Hidden(nameof(Parameters.AudioLengthSeconds))
            .Build();

        /// <summary> The report from the last run, for a host to show once the level exists. It is
        /// not part of GeneratedLevel because that struct is the format's, not this converter's. </summary>
        public InteropReport LastReport { get; private set; }

        protected override GeneratedLevel CreateTyped(Parameters parameters)
        {
            var options = ToOptions(parameters);
            var result = ABInterop.ImportLevel(parameters.LevelJson, parameters.MetaJson, options);
            LastReport = result.Report;

            if (result.Level == null)
            {
                // Handed nothing usable, produce nothing usable - an empty level the author can see
                // is empty, rather than one that looks like a failed conversion of their content.
                var empty = new Level();
                empty.Settings.Framerate = options.Framerate;
                return new GeneratedLevel(empty, new LevelMeta());
            }

            AttachAudio(result.Level, parameters, result.Report);
            return new GeneratedLevel(result.Level, result.Meta);
        }

        // Afterbeat keeps its song beside the level file with no reference to it anywhere in the
        // documents, so the track is built here rather than in the importer: the file name is
        // something only the host that opened the folder knows.
        private static void AttachAudio(Level level, Parameters parameters, InteropReport report)
        {
            if (string.IsNullOrEmpty(parameters.AudioFileName))
            {
                report.Info("audio_missing",
                    "No song was found beside the level, so the imported level has no audio track. Add one in the audio timeline.",
                    parameters.SourceFolder);
                return;
            }

            var resourceId = new AudioResourceId(AudioResourceId.MaxUserDefinedValue);
            level.Resources.Audios[resourceId] = new AudioResource(resourceId,
                new List<ResourceKey> { new(ResourceUriType.LevelPath, parameters.AudioFileName) });

            var audioId = level.Settings.GetNextAudioId();
            level.Audio.Tracks[audioId] = new LevelTrack
            {
                AudioId = audioId,
                AudioResourceId = resourceId,
                Span = new FrameSpan(FrameRules.MinFrame, level.Settings.FrameDuration),
                Name = parameters.AudioFileName,
            };
        }

        // The level's length is only known after the documents are read, so an estimate before that
        // can only count what the source itself carries. Reading the .vgd twice is cheap next to
        // showing the author a number that has nothing to do with their level.
        protected override GeneratorCost EstimateTyped(Parameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.LevelJson)) return GeneratorCost.Zero;
            if (!ABSerialization.TryDeserialize<VgdLevel>(parameters.LevelJson, out var source, out _))
                return GeneratorCost.Zero;

            var objects = source.Objects?.Count ?? 0;
            if (parameters.ImportPrefabs) objects += source.PrefabPlacements?.Count ?? 0;
            if (parameters.ImportParallax && source.Parallax?.Layers != null)
                foreach (var layer in source.Parallax.Layers)
                    objects += layer?.Objects?.Count ?? 0;

            var resources = (source.Themes?.Count ?? 0)
                            + CountDefaultThemes(source)
                            + (parameters.ImportPrefabs ? source.Prefabs?.Count ?? 0 : 0);

            return new GeneratorCost(objects, 0, resources);
        }

        // A level using one of the game's own palettes carries no themes[] entry for it at all, so
        // the estimate has to count the ones the import will materialize or a whole-level conversion
        // reads as costing no resources.
        private static int CountDefaultThemes(VgdLevel source)
        {
            var counted = new HashSet<string>();

            foreach (var key in source.GetEvents(ABEventTrack.Theme))
            {
                var id = key?.GetString(0);
                if (ABDefaultThemes.Contains(id)) counted.Add(id);
            }

            return counted.Count;
        }

        private static ABOptions ToOptions(Parameters parameters) => new()
        {
            Framerate = parameters.Framerate,
            ImportParallax = parameters.ImportParallax,
            ParallaxActive = parameters.ParallaxActive,
            ImportPrefabs = parameters.ImportPrefabs,
            KeepObjectNames = parameters.KeepObjectNames,
            ParallaxLayerOffset = parameters.ParallaxLayerOffset,
            MaxParallaxLoopKeys = parameters.MaxParallaxLoopKeys,
            LayerImport = parameters.LayerImport,
            EditorGroupStride = parameters.EditorGroupStride,
            PlacementLayerOffset = parameters.PlacementLayerOffset,
            AudioLengthSeconds = parameters.AudioLengthSeconds,
            OpacityHitThreshold = parameters.OpacityHitThreshold,
        };

        /// <summary> Public mutable fields, like every parameters class here - a form binds to them
        /// and a preset serializes from them. </summary>
        public class Parameters : IABLevelInput
        {
            /// <summary> Afterbeat stores time in seconds; this is what those seconds are resolved
            /// into. Higher keeps keyframes that sit close together apart. </summary>
            public int Framerate = ABOptions.DefaultFramerate;

            public bool ImportParallax = true;
            public bool ImportPrefabs = true;
            public bool KeepObjectNames = true;

            /// <summary> The alpha an object has to be drawn at to hurt the player. One - the
            /// default - is Afterbeat's own rule, and the only value that reproduces the source
            /// level; lower keeps a fading object lethal for more of its fade, and zero leaves every
            /// collider exactly as the source had it. See
            /// <see cref="ABOptions.OpacityHitThreshold"/>. </summary>
            public float OpacityHitThreshold = ABOptions.DefaultOpacityHitThreshold;

            /// <summary> Afterbeat sorts by an absolute depth and organises a level into editor
            /// layers this format has no field for, so what a converted level draws in front - and
            /// how many timeline rows it arrives on - is a choice. See
            /// <see cref="ABLayerImport"/>. </summary>
            public ABLayerImport LayerImport = ABLayerImport.Auto;

            public int EditorGroupStride = ABLayerMap.DepthSpan;
            public int PlacementLayerOffset;

            /// <summary> Whether the imported background arrives switched on. Off - the default -
            /// keeps every background object and its baked loop while leaving the level looking
            /// like its own content; see <see cref="ABOptions.ParallaxActive"/>. </summary>
            public bool ParallaxActive;

            public int ParallaxLayerOffset = 1;
            public int MaxParallaxLoopKeys = LevelRules.MaxObjectKeys;

            /// <summary> Filled in by the host from the folder it opened - see
            /// <see cref="IABLevelInput"/>. Fields with explicit interface forwarding, the
            /// same shape every other external input in this folder takes: a form binds to fields,
            /// and an interface member is not one. </summary>
            public string LevelJson = string.Empty;
            public string MetaJson = string.Empty;
            public string AudioFileName = string.Empty;
            public string SourceFolder = string.Empty;
            public float AudioLengthSeconds;

            string IABLevelInput.LevelJson
            {
                get => LevelJson;
                set => LevelJson = value;
            }
            string IABLevelInput.MetaJson
            {
                get => MetaJson;
                set => MetaJson = value;
            }
            string IABLevelInput.AudioFileName
            {
                get => AudioFileName;
                set => AudioFileName = value;
            }
            string IABLevelInput.SourceFolder
            {
                get => SourceFolder;
                set => SourceFolder = value;
            }
            float IABLevelInput.AudioLengthSeconds
            {
                get => AudioLengthSeconds;
                set => AudioLengthSeconds = value;
            }
        }
    }
}
