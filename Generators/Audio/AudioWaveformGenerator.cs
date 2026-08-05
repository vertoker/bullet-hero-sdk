using BH.SDK.Generators.External;
using BH.SDK.Generators.Spawn;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Audio
{
    // Peaks come from the host (Unity side: Timelines/Utils/AudioWaveformCache, which already
    // downsamples a clip for the audio timeline) rather than being computed here - the SDK has no
    // decoder. With no peaks supplied this produces nothing at all, deliberately: a waveform of
    // invented numbers is worse than an empty run, because it looks like it worked.

    /// <summary>
    /// A bar chart of a track's waveform, as real level objects - the song's shape becomes scenery.
    /// </summary>
    public class AudioWaveformGenerator : BaseSpawnGenerator<AudioWaveformGenerator.Parameters>
    {
        public override string NameKey => "gen_audio_waveform";

        public override GeneratorRequirements Requirements => GeneratorRequirements.ExternalAnalysis;

        public override GeneratorHints Hints { get; } = new GeneratorHints.Builder()
            .Order(nameof(Parameters.BarCount), nameof(Parameters.BarWidth), nameof(Parameters.Spacing),
                nameof(Parameters.Height), nameof(Parameters.MinHeight),
                nameof(Parameters.OriginX), nameof(Parameters.OriginY),
                nameof(Parameters.Mirrored), nameof(Parameters.Source), nameof(Parameters.Peaks))
            .Order(SpawnParameters.FieldOrder)
            .Range(nameof(Parameters.BarCount), 0, 512)
            .Range(nameof(Parameters.BarWidth), 0.01f, ValueRules.MaxSca)
            .Range(nameof(Parameters.Spacing), 0f, ValueRules.MaxPos)
            .Range(nameof(Parameters.Height), 0.01f, ValueRules.MaxSca)
            .Range(nameof(Parameters.MinHeight), 0f, ValueRules.MaxSca)
            // Source stays visible - which track to visualize is an authoring choice; the sampled
            // peaks behind it are not.
            .Hidden(nameof(Parameters.Peaks))
            .Build();

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var bars = BarCount(parameters);
            if (bars <= 0) return;

            var totalWidth = bars * parameters.BarWidth + (bars - 1) * parameters.Spacing;
            var left = parameters.OriginX - totalWidth * 0.5f + parameters.BarWidth * 0.5f;

            for (var i = 0; i < bars; i++)
            {
                var peak = PeakAt(parameters, bars, i);
                var height = parameters.MinHeight + peak * parameters.Height;
                if (parameters.Mirrored) height *= 2f;

                var x = left + i * (parameters.BarWidth + parameters.Spacing);

                // A non-mirrored bar grows upward from the baseline, so its centre sits half a
                // height above it; a mirrored one is centred on the baseline itself.
                var y = parameters.Mirrored ? parameters.OriginY : parameters.OriginY + height * 0.5f;

                var obj = Spawn(context, parameters, $"waveform_{i}", context.StartFrame, context.EndFrame);
                AddPosition(obj, x, y, obj.StartFrame);
                SetSize(obj, parameters.BarWidth, height);
            }
        }

        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        {
            var bars = BarCount(parameters);
            return new GeneratorCost(bars, bars * KeysPerBar);
        }

        /// <summary> Bars actually drawn: the author's requested count, capped by how many peaks the
        /// host supplied, and defaulting to one bar per peak when no count is set. </summary>
        private static int BarCount(Parameters parameters)
        {
            var available = parameters.Peaks?.Length ?? 0;
            if (available <= 0) return 0;
            if (parameters.BarCount <= 0) return available;
            return parameters.BarCount < available ? parameters.BarCount : available;
        }

        // Bars are resampled from the peak array rather than read one-to-one, so asking for fewer
        // bars than there are peaks summarises the whole track instead of showing only its start.
        private static float PeakAt(Parameters parameters, int bars, int index)
        {
            var peaks = parameters.Peaks;
            if (peaks == null || peaks.Length == 0) return 0f;

            var from = (int)((long)index * peaks.Length / bars);
            var to = (int)((long)(index + 1) * peaks.Length / bars);
            if (to <= from) to = from + 1;
            if (to > peaks.Length) to = peaks.Length;

            var max = 0f;
            for (var i = from; i < to; i++)
            {
                var value = peaks[i] < 0f ? -peaks[i] : peaks[i];
                if (value > max) max = value;
            }
            return max > 1f ? 1f : max;
        }

        private const int KeysPerBar = 3; // position + size + colour

        public class Parameters : SpawnParameters, IWaveformInput
        {
            /// <summary> Zero means "one bar per supplied peak". </summary>
            public int BarCount = 64;
            public float BarWidth = 0.2f;
            public float Spacing = 0.05f;
            public float Height = 6f;
            public float MinHeight = 0.1f;
            public float OriginX;
            public float OriginY = -6f;
            public bool Mirrored;

            public AudioResourceId Source = AudioResourceId.Null;
            public float[] Peaks = System.Array.Empty<float>();

            AudioResourceId IWaveformInput.Source
            {
                get => Source;
                set => Source = value;
            }
            float[] IWaveformInput.Peaks
            {
                get => Peaks;
                set => Peaks = value;
            }
        }
    }
}
