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
    // Interop/AfterBeat/AfterBeatInterop.ExportLevel).
    //
    // ExternalAnalysis for the same reason gen_level_audio_file needs it: the SDK reads no files.
    // The host opens the folder and fills in IAfterBeatLevelInput before this runs. Handed nothing,
    // this produces an empty level and says why - never a plausible-looking one.

    /// <summary> Builds a level out of an Afterbeat (Project Arrhythmia) level folder. </summary>
    public class AfterBeatLevelGenerator : BaseLevelGenerator<AfterBeatLevelGenerator.Parameters>
    {
        public override string NameKey => "gen_level_afterbeat";

        public override GeneratorRequirements Requirements => GeneratorRequirements.ExternalAnalysis;

        public override GeneratorHints Hints => HintsValue;

        private static readonly GeneratorHints HintsValue = new GeneratorHints.Builder()
            .Section(GeneratorSections.Main, nameof(Parameters.Framerate),
                nameof(Parameters.ImportParallax), nameof(Parameters.ImportPrefabs))
            // The host-filled fields are listed like any other - Hidden decides whether a row is
            // SHOWN, not whether the field is accounted for, and a field in no section still
            // renders, at the bottom, where nobody would look for it.
            .Section(GeneratorSections.Additional, nameof(Parameters.KeepObjectNames),
                nameof(Parameters.ParallaxBaseLayer), nameof(Parameters.MaxParallaxLoopKeys),
                nameof(Parameters.LevelJson), nameof(Parameters.MetaJson),
                nameof(Parameters.AudioFileName), nameof(Parameters.SourceFolder))
            .Range(nameof(Parameters.Framerate), FrameRules.MinFramerate, FrameRules.MaxFramerate)
            .Range(nameof(Parameters.ParallaxBaseLayer), ValueRules.MinLayer, ValueRules.MaxLayer)
            .Range(nameof(Parameters.MaxParallaxLoopKeys), 2, LevelRules.MaxObjectKeys)
            .Unit(nameof(Parameters.Framerate), "fps")
            .Unit(nameof(Parameters.MaxParallaxLoopKeys), "keys")
            // The host fills these in from the folder it opened; showing them as editable rows would
            // invite an author to paste a level document into a text field.
            .Hidden(nameof(Parameters.LevelJson))
            .Hidden(nameof(Parameters.MetaJson))
            .Hidden(nameof(Parameters.AudioFileName))
            .Hidden(nameof(Parameters.SourceFolder))
            .Build();

        /// <summary> The report from the last run, for a host to show once the level exists. It is
        /// not part of GeneratedLevel because that struct is the format's, not this converter's. </summary>
        public InteropReport LastReport { get; private set; }

        protected override GeneratedLevel CreateTyped(Parameters parameters)
        {
            var options = ToOptions(parameters);
            var result = AfterBeatInterop.ImportLevel(parameters.LevelJson, parameters.MetaJson, options);
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
            if (!AfterBeatSerialization.TryDeserialize<VgdLevel>(parameters.LevelJson, out var source, out _))
                return GeneratorCost.Zero;

            var objects = source.Objects?.Count ?? 0;
            if (parameters.ImportPrefabs) objects += source.PrefabPlacements?.Count ?? 0;
            if (parameters.ImportParallax && source.Parallax?.Layers != null)
                foreach (var layer in source.Parallax.Layers)
                    objects += layer?.Objects?.Count ?? 0;

            var resources = (source.Themes?.Count ?? 0)
                            + (parameters.ImportPrefabs ? source.Prefabs?.Count ?? 0 : 0);

            return new GeneratorCost(objects, 0, resources);
        }

        private static AfterBeatOptions ToOptions(Parameters parameters) => new()
        {
            Framerate = parameters.Framerate,
            ImportParallax = parameters.ImportParallax,
            ImportPrefabs = parameters.ImportPrefabs,
            KeepObjectNames = parameters.KeepObjectNames,
            ParallaxBaseLayer = parameters.ParallaxBaseLayer,
            MaxParallaxLoopKeys = parameters.MaxParallaxLoopKeys,
        };

        /// <summary> Public mutable fields, like every parameters class here - a form binds to them
        /// and a preset serializes from them. </summary>
        public class Parameters : IAfterBeatLevelInput
        {
            /// <summary> Afterbeat stores time in seconds; this is what those seconds are resolved
            /// into. Higher keeps keyframes that sit close together apart. </summary>
            public int Framerate = AfterBeatOptions.DefaultFramerate;

            public bool ImportParallax = true;
            public bool ImportPrefabs = true;
            public bool KeepObjectNames = true;

            public int ParallaxBaseLayer = -100;
            public int MaxParallaxLoopKeys = LevelRules.MaxObjectKeys;

            /// <summary> Filled in by the host from the folder it opened - see
            /// <see cref="IAfterBeatLevelInput"/>. Fields with explicit interface forwarding, the
            /// same shape every other external input in this folder takes: a form binds to fields,
            /// and an interface member is not one. </summary>
            public string LevelJson = string.Empty;
            public string MetaJson = string.Empty;
            public string AudioFileName = string.Empty;
            public string SourceFolder = string.Empty;

            string IAfterBeatLevelInput.LevelJson
            {
                get => LevelJson;
                set => LevelJson = value;
            }
            string IAfterBeatLevelInput.MetaJson
            {
                get => MetaJson;
                set => MetaJson = value;
            }
            string IAfterBeatLevelInput.AudioFileName
            {
                get => AudioFileName;
                set => AudioFileName = value;
            }
            string IAfterBeatLevelInput.SourceFolder
            {
                get => SourceFolder;
                set => SourceFolder = value;
            }
        }
    }
}
