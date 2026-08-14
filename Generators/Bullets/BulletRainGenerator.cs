using BH.SDK.Generators.Spawn;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Bullets
{
    // The only generator so far whose output is not a formula, which makes it the one that proves
    // determinism actually works: the same context seed has to reproduce the same rain exactly, or
    // an author's saved level stops matching what they authored. Randomness comes from
    // GeneratorContext.CreateRandom(), never from System.Random.

    /// <summary>
    /// Bullets falling through a band of the screen, scattered across it and staggered in time.
    /// </summary>
    public class BulletRainGenerator : BaseSpawnGenerator<BulletRainGenerator.Parameters>
    {
        public override string NameKey => "gen_bullet_rain";

        public override GeneratorHints Hints { get; } = new GeneratorHints.Builder()
            .Section(GeneratorSections.Main, SpawnParameters.MainFields)
            .Section(GeneratorSections.Main, nameof(Parameters.Count), nameof(Parameters.TravelFrames))
            .Section(GeneratorSections.Additional, SpawnParameters.AdditionalFields)
            .Section(GeneratorSections.Additional, nameof(Parameters.AreaLeft), nameof(Parameters.AreaRight),
                nameof(Parameters.TopY), nameof(Parameters.BottomY), nameof(Parameters.SpreadFrames),
                nameof(Parameters.TravelJitter), nameof(Parameters.Ease))
            .Range(nameof(Parameters.Count), 1, 1024)
            .Range(nameof(Parameters.AreaLeft), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(Parameters.AreaRight), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(Parameters.TopY), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(Parameters.BottomY), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(Parameters.TravelFrames), 1, FrameRules.MaxFrameDuration)
            .Range(nameof(Parameters.SpreadFrames), 0, FrameRules.MaxFrameDuration)
            .Range(nameof(Parameters.TravelJitter), 0f, 1f)
            .Unit(nameof(Parameters.TravelFrames), "frames")
            .Unit(nameof(Parameters.SpreadFrames), "frames")
            .Range(nameof(SpawnParameters.Size), ValueRules.MinSca, ValueRules.MaxSca)
            .Build();

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var random = context.CreateRandom();

            for (var i = 0; i < count; i++)
            {
                // Every draw happens in the same order for every run, including the ones whose
                // result is then clamped away - pulling numbers conditionally would make the
                // sequence depend on the parameters and break "same seed, same rain".
                var x = random.NextFloat(parameters.AreaLeft, parameters.AreaRight);
                var delay = random.NextInt(0, Spread(parameters.SpreadFrames) + 1);
                var jitter = random.NextFloat(1f - Jitter(parameters.TravelJitter), 1f);

                var travel = (int)(Travel(parameters.TravelFrames) * jitter);
                if (travel < 1) travel = 1;

                var spawnFrame = context.Span.StartFrame + delay;
                if (!CanSpawn(context, spawnFrame)) continue; // scattered past the window - drop it, do not stack it on the end
                var obj = Spawn(context, parameters, $"rain_{i}", new FrameSpan(spawnFrame, travel));
                AddPosition(obj, x, parameters.TopY, obj.Span.StartFrame);

                if (CanAnimate(obj.Span))
                    AddPosition(obj, x, parameters.BottomY, obj.Span.LastFrame, parameters.Ease);
            }
        }

        // Mirrors Generate's own draw order exactly, because the number of position keys depends on
        // the randomly-chosen delay - an estimate that assumed two keys each would drift near the
        // end of the context window, where lifetimes get clamped.
        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var random = context.CreateRandom();
            var keys = 0;
            var objects = 0;

            for (var i = 0; i < count; i++)
            {
                random.NextFloat(parameters.AreaLeft, parameters.AreaRight);
                var delay = random.NextInt(0, Spread(parameters.SpreadFrames) + 1);
                var jitter = random.NextFloat(1f - Jitter(parameters.TravelJitter), 1f);

                var travel = (int)(Travel(parameters.TravelFrames) * jitter);
                if (travel < 1) travel = 1;

                if (!CanSpawn(context, context.Span.StartFrame + delay)) continue;
                objects++;

                var span = ClampSpan(context, new FrameSpan(ClampFrame(context, context.Span.StartFrame + delay), travel));
                keys += 2 + (CanAnimate(span) ? 2 : 1); // size + colour + position(s)
            }
            return new GeneratorCost(objects, keys);
        }

        private static int Count(int value) => value < 1 ? 1 : value;
        private static int Travel(int value) => value < 1 ? 1 : value;
        private static int Spread(int value) => value < 0 ? 0 : value;
        private static float Jitter(float value) => value < 0f ? 0f : value > 1f ? 1f : value;

        public class Parameters : SpawnParameters
        {
            public int Count = 24;
            public float AreaLeft = -12f;
            public float AreaRight = 12f;
            public float TopY = 8f;
            public float BottomY = -8f;
            public int TravelFrames = 75;
            public int SpreadFrames = 60;
            public float TravelJitter = 0.25f;
            public EaseType Ease = EaseType.Linear;
        }
    }
}
