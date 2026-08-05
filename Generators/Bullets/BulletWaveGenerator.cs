using System;
using BH.SDK.Generators.Spawn;
using BH.SDK.Models.Enum;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Bullets
{
    // The first of the animated generators, and the one that sets the pattern for the rest: an
    // object's lifetime is [spawn, spawn + travel] rather than the whole context range, so a wave
    // that fires over two seconds does not leave 60 permanently-alive objects sitting on the
    // timeline. Frames are clamped to the context window by Spawn, so a stagger overshooting the
    // range is truncated rather than leaking outside it.

    /// <summary>
    /// A line of bullets travelling from one point to another, optionally staggered so they arrive
    /// one after the other instead of as a wall.
    /// </summary>
    public class BulletWaveGenerator : BaseSpawnGenerator<BulletWaveGenerator.Parameters>
    {
        public override string NameKey => "gen_bullet_wave";

        public override GeneratorHints Hints { get; } = new GeneratorHints.Builder()
            .Order(nameof(Parameters.Count), nameof(Parameters.Spacing),
                nameof(Parameters.FromX), nameof(Parameters.FromY),
                nameof(Parameters.ToX), nameof(Parameters.ToY),
                nameof(Parameters.TravelFrames), nameof(Parameters.StaggerFrames),
                nameof(Parameters.Ease), nameof(Parameters.FaceTravel))
            .Order(SpawnParameters.FieldOrder)
            .Range(nameof(Parameters.Count), 1, 512)
            .Range(nameof(Parameters.Spacing), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(Parameters.TravelFrames), 1, FrameRules.MaxFrameLength)
            .Range(nameof(Parameters.StaggerFrames), 0, FrameRules.MaxFrameLength)
            .Unit(nameof(Parameters.TravelFrames), "frames")
            .Unit(nameof(Parameters.StaggerFrames), "frames")
            .Build();

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var travel = Travel(parameters.TravelFrames);
            var stagger = Stagger(parameters.StaggerFrames);

            var dirX = parameters.ToX - parameters.FromX;
            var dirY = parameters.ToY - parameters.FromY;
            var length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            // Bullets are spread ACROSS the travel direction, not along it - a wave is a line
            // perpendicular to where it is going. A zero-length travel has no direction to be
            // perpendicular to, so it falls back to spreading along X.
            var normalX = length > 0f ? -dirY / length : 0f;
            var normalY = length > 0f ? dirX / length : 1f;
            if (length <= 0f)
            {
                normalX = 1f;
                normalY = 0f;
            }

            var angle = (float)(Math.Atan2(dirY, dirX) * (180.0 / Math.PI));

            for (var i = 0; i < count; i++)
            {
                var offset = (i - (count - 1) * 0.5f) * parameters.Spacing;
                var spawnFrame = context.StartFrame + i * stagger;

                var obj = Spawn(context, parameters, $"wave_{i}", spawnFrame, spawnFrame + travel);
                AddPosition(obj, parameters.FromX + normalX * offset, parameters.FromY + normalY * offset,
                    obj.StartFrame);

                // A bullet whose lifetime the context window truncated to a single frame gets only
                // its start position - a second key on the same frame would break Frame uniqueness.
                if (CanAnimate(obj.StartFrame, obj.EndFrame))
                    AddPosition(obj, parameters.ToX + normalX * offset, parameters.ToY + normalY * offset,
                        obj.EndFrame, parameters.Ease);

                if (parameters.FaceTravel) AddRotation(obj, angle, obj.StartFrame);
            }
        }

        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var travel = Travel(parameters.TravelFrames);
            var stagger = Stagger(parameters.StaggerFrames);
            var perObject = 2 + (parameters.FaceTravel ? 1 : 0); // size + colour (+ rotation)

            var keys = 0;
            for (var i = 0; i < count; i++)
            {
                var spawnFrame = ClampFrame(context, context.StartFrame + i * stagger);
                var endFrame = ClampFrame(context, spawnFrame + travel);
                keys += perObject + (CanAnimate(spawnFrame, endFrame) ? 2 : 1);
            }
            return new GeneratorCost(count, keys);
        }

        private static int Count(int value) => value < 1 ? 1 : value;
        private static int Travel(int value) => value < 1 ? 1 : value;
        private static int Stagger(int value) => value < 0 ? 0 : value;

        public class Parameters : SpawnParameters
        {
            public int Count = 8;
            public float Spacing = 1.5f;
            public float FromX = -10f;
            public float FromY = 6f;
            public float ToX = -10f;
            public float ToY = -6f;
            public int TravelFrames = 60;
            public int StaggerFrames = 4;
            public EaseType Ease = EaseType.Linear;
            public bool FaceTravel;
        }
    }
}
