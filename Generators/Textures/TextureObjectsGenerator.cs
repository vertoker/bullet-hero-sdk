using System.Collections.Generic;
using BH.SDK.Generators.External;
using BH.SDK.Generators.Spawn;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Utils;

namespace BH.SDK.Generators.Textures
{
    // Replaces the original TextureToObjectsGenerator, which was broken in three separate ways: its
    // pixel objects were never added to the scope (only re-parented), the parent's Layer was
    // assigned inside the per-pixel loop, and its DimensionalIndexer2 was built from the TARGET size
    // while indexing the SOURCE pixels. All three are the kind of bug that a per-object estimate and
    // a validation test catch immediately, which is why this rewrite has both.
    //
    // The other lesson from the original: one object per pixel is unusable. A 512x512 image is
    // 262 144 objects - exactly LevelRules.MaxObjects, from one click. Downsampling bounds the
    // input, and run-length merging collapses flat areas into single wide rects, which is what makes
    // pixel art actually affordable.

    /// <summary>
    /// Turns an image into level objects: downsampled, transparent pixels skipped, and horizontal
    /// runs of one colour merged into single rectangles.
    /// </summary>
    public class TextureObjectsGenerator : BaseSpawnGenerator<TextureObjectsGenerator.Parameters>
    {
        public override string NameKey => "gen_texture_objects";

        public override GeneratorRequirements Requirements => GeneratorRequirements.ExternalAnalysis;

        public override GeneratorHints Hints { get; } = new GeneratorHints.Builder()
            .Order(nameof(Parameters.TargetWidth), nameof(Parameters.TargetHeight),
                nameof(Parameters.PixelSize), nameof(Parameters.OriginX), nameof(Parameters.OriginY),
                nameof(Parameters.AlphaThreshold), nameof(Parameters.MergeRuns),
                nameof(Parameters.UseThemeRef), nameof(Parameters.Theme),
                nameof(Parameters.Source), nameof(Parameters.Texture))
            .Order(SpawnParameters.FieldOrder)
            .Range(nameof(Parameters.TargetWidth), 1, MaxSide)
            .Range(nameof(Parameters.TargetHeight), 1, MaxSide)
            .Range(nameof(Parameters.PixelSize), 0.01f, ValueRules.MaxSca)
            .Range(nameof(Parameters.AlphaThreshold), 0f, 1f)
            .VisibleWhen(nameof(Parameters.Theme), p => ((Parameters)p).UseThemeRef)
            // Source stays VISIBLE - which image to convert is the author's decision. Only the
            // decoded pixels behind it are the host's, and those are what Texture holds.
            .Hidden(nameof(Parameters.Texture))
            // Size and Collider come from SpawnParameters but mean nothing here: every object is
            // sized to its own run, and a picture is decoration.
            .Hidden(nameof(SpawnParameters.Size))
            .Build();

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var runs = BuildRuns(parameters);
            if (runs.Count == 0) return;

            var palette = parameters.UseThemeRef ? ResolvePalette(context, parameters) : null;
            var width = TargetWidth(parameters);
            var height = TargetHeight(parameters);

            // The image is centred on the origin, and row 0 of the source is the TOP row - hence
            // the negated Y. Getting this wrong flips every generated picture upside down.
            var offsetX = parameters.OriginX - (width - 1) * parameters.PixelSize * 0.5f;
            var offsetY = parameters.OriginY + (height - 1) * parameters.PixelSize * 0.5f;

            var index = 0;
            foreach (var run in runs)
            {
                var obj = Spawn(context, parameters, $"pixels_{index++}", context.StartFrame, context.EndFrame);
                var centerX = offsetX + (run.X + (run.Length - 1) * 0.5f) * parameters.PixelSize;
                var centerY = offsetY - run.Y * parameters.PixelSize;

                AddPosition(obj, centerX, centerY, obj.StartFrame);
                SetSize(obj, run.Length * parameters.PixelSize, parameters.PixelSize);

                obj.Colors.Clear();
                AddColor(obj, ResolveColor(run.Color, palette), obj.StartFrame);
            }
        }

        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        {
            var runs = BuildRuns(parameters);
            return new GeneratorCost(runs.Count, runs.Count * KeysPerRun);
        }

        /// <summary> One merged horizontal run of same-coloured pixels. </summary>
        private readonly struct Run
        {
            public readonly int X;
            public readonly int Y;
            public readonly int Length;
            public readonly Pixel Color;

            public Run(int x, int y, int length, Pixel color)
            {
                X = x;
                Y = y;
                Length = length;
                Color = color;
            }
        }

        // Generate and Estimate walk this same list, so the estimate cannot drift from the run: the
        // merging decisions ARE the object count, and there is no closed form for them.
        private static List<Run> BuildRuns(Parameters parameters)
        {
            var runs = new List<Run>();
            var source = parameters.Texture;
            if (source?.Pixels == null || source.Width <= 0 || source.Height <= 0) return runs;

            var width = TargetWidth(parameters);
            var height = TargetHeight(parameters);
            var threshold = (byte)(Clamp01(parameters.AlphaThreshold) * byte.MaxValue);

            for (var y = 0; y < height; y++)
            {
                var runStart = -1;
                var runColor = default(Pixel);

                for (var x = 0; x < width; x++)
                {
                    var pixel = Sample(source, parameters, width, height, x, y);
                    var visible = pixel.a > threshold;
                    var continues = runStart >= 0 && visible && parameters.MergeRuns
                                    && pixel.rgba == runColor.rgba;

                    if (continues) continue;

                    if (runStart >= 0) runs.Add(new Run(runStart, y, x - runStart, runColor));
                    if (!visible)
                    {
                        runStart = -1;
                        continue;
                    }
                    runStart = x;
                    runColor = pixel;
                }

                if (runStart >= 0) runs.Add(new Run(runStart, y, width - runStart, runColor));
            }
            return runs;
        }

        // Box-average downsample: every target pixel is the mean of the source block behind it, so
        // shrinking an image blurs it rather than dropping three quarters of it. Averaging happens
        // in premultiplied alpha, otherwise a transparent pixel's (arbitrary) colour bleeds into
        // its visible neighbours.
        private static Pixel Sample(PixelTexture source, Parameters parameters, int width, int height,
            int x, int y)
        {
            var fromX = (int)((long)x * source.Width / width);
            var toX = (int)((long)(x + 1) * source.Width / width);
            var fromY = (int)((long)y * source.Height / height);
            var toY = (int)((long)(y + 1) * source.Height / height);
            if (toX <= fromX) toX = fromX + 1;
            if (toY <= fromY) toY = fromY + 1;
            if (toX > source.Width) toX = source.Width;
            if (toY > source.Height) toY = source.Height;

            long r = 0, g = 0, b = 0, a = 0;
            var count = 0;
            for (var sy = fromY; sy < toY; sy++)
            for (var sx = fromX; sx < toX; sx++)
            {
                var pixel = source.Pixels[sy * source.Width + sx];
                r += (long)pixel.r * pixel.a;
                g += (long)pixel.g * pixel.a;
                b += (long)pixel.b * pixel.a;
                a += pixel.a;
                count++;
            }
            if (count == 0 || a == 0) return new Pixel(0, 0, 0, 0);

            return new Pixel((byte)(r / a), (byte)(g / a), (byte)(b / a), (byte)(a / count));
        }

        /// <summary> The active theme's colours, or null when theme mapping is off or the referenced
        /// theme is missing - in which case literal colours are used, not a broken reference. </summary>
        private static Color4Value[] ResolvePalette(GeneratorContext context, Parameters parameters)
        {
            if (context?.Resources == null) return null;
            if (!context.Resources.Themes.TryGetValue(parameters.Theme, out var theme)) return null;
            return theme.Matrix;
        }

        // Mapping to a ThemeRef is what lets a generated picture follow the level's palette instead
        // of freezing whatever the source image happened to contain. Nearest match in plain RGB -
        // a perceptual distance would be better, but this is a colour picker, not a print pipeline.
        private static IColor4 ResolveColor(Pixel pixel, Color4Value[] palette)
        {
            var literal = pixel.ToColorValue();
            if (palette == null || palette.Length == 0) return literal;

            var bestIndex = 0;
            var bestDistance = float.MaxValue;
            for (var i = 0; i < palette.Length; i++)
            {
                var candidate = palette[i];
                if (candidate == null) continue;

                var dr = candidate.R - literal.R;
                var dg = candidate.G - literal.G;
                var db = candidate.B - literal.B;
                var distance = dr * dr + dg * dg + db * db;
                if (distance >= bestDistance) continue;

                bestDistance = distance;
                bestIndex = i;
            }
            return new Color4ThemeRef(bestIndex);
        }

        private const int MaxSide = 256;
        private const int KeysPerRun = 3; // position + size + colour

        private static int TargetWidth(Parameters parameters)
            => Side(parameters.TargetWidth, parameters.Texture?.Width ?? 0);
        private static int TargetHeight(Parameters parameters)
            => Side(parameters.TargetHeight, parameters.Texture?.Height ?? 0);

        /// <summary> A requested side of zero means "use the source's own", still capped. </summary>
        private static int Side(int requested, int sourceSide)
        {
            var value = requested > 0 ? requested : sourceSide;
            if (value < 1) value = 1;
            return value > MaxSide ? MaxSide : value;
        }

        private static float Clamp01(float value) => value < 0f ? 0f : value > 1f ? 1f : value;

        public class Parameters : SpawnParameters, IPixelTextureInput
        {
            /// <summary> Zero means "the source image's own size", capped at 256. </summary>
            public int TargetWidth = 64;
            public int TargetHeight = 64;
            public float PixelSize = 0.25f;
            public float OriginX;
            public float OriginY;
            public float AlphaThreshold = 0.05f;
            public bool MergeRuns = true;
            public bool UseThemeRef;
            public ThemeId Theme = ThemeId.Null;

            public TextureResourceId Source = TextureResourceId.Null;
            public PixelTexture Texture;

            TextureResourceId IPixelTextureInput.Source
            {
                get => Source;
                set => Source = value;
            }
            PixelTexture IPixelTextureInput.Texture
            {
                get => Texture;
                set => Texture = value;
            }
        }
    }
}
