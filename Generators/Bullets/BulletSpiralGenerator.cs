using BH.SDK.Generators.Spawn;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Bullets
{
    /// <summary>
    /// Bullets fired outward from a point, each rotated one step further than the last, so a
    /// staggered burst reads as a rotating spray rather than a ring.
    /// </summary>
    public class BulletSpiralGenerator : BaseSpawnGenerator<BulletSpiralGenerator.Parameters>
    {
        public override string NameKey => "gen_bullet_spiral";

        public override GeneratorHints Hints { get; } = new GeneratorHints.Builder()
            .Section(GeneratorSections.Main, SpawnParameters.MainFields)
            .Section(GeneratorSections.Main, nameof(Parameters.Count), nameof(Parameters.AngularStep),
                nameof(Parameters.RadiusStart), nameof(Parameters.RadiusEnd),
                nameof(Parameters.TravelFrames))
            .Section(GeneratorSections.Additional, SpawnParameters.AdditionalFields)
            .Section(GeneratorSections.Additional, nameof(Parameters.StartAngle),
                nameof(Parameters.CenterX), nameof(Parameters.CenterY),
                nameof(Parameters.StaggerFrames), nameof(Parameters.Ease), nameof(Parameters.FaceOutward))
            .Range(nameof(Parameters.Count), 1, 512)
            .Range(nameof(Parameters.AngularStep), -360f, 360f)
            .Range(nameof(Parameters.RadiusStart), 0f, ValueRules.MaxPos)
            .Range(nameof(Parameters.RadiusEnd), 0f, ValueRules.MaxPos)
            .Range(nameof(Parameters.TravelFrames), 1, FrameRules.MaxFrameDuration)
            .Range(nameof(Parameters.StaggerFrames), 0, FrameRules.MaxFrameDuration)
            .Unit(nameof(Parameters.AngularStep), "deg")
            .Unit(nameof(Parameters.StartAngle), "deg")
            .Unit(nameof(Parameters.TravelFrames), "frames")
            .Unit(nameof(Parameters.StaggerFrames), "frames")
            .Range(nameof(Parameters.StartAngle), -3600f, 3600f)
            .Range(nameof(Parameters.CenterX), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(Parameters.CenterY), ValueRules.MinPos, ValueRules.MaxPos)
            .Range(nameof(SpawnParameters.Size), ValueRules.MinSca, ValueRules.MaxSca)
            .Build();

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var travel = Travel(parameters.TravelFrames);
            var stagger = Stagger(parameters.StaggerFrames);

            for (var i = 0; i < count; i++)
            {
                var angle = parameters.StartAngle + parameters.AngularStep * i;
                Direction(angle, out var dirX, out var dirY);
                var spawnFrame = context.Span.StartFrame + i * stagger;
                if (!CanSpawn(context, spawnFrame)) break; // stagger ran past the window - no ghost on its last frame

                var obj = Spawn(context, parameters, $"spiral_{i}", new FrameSpan(spawnFrame, travel));
                AddPosition(obj,
                    parameters.CenterX + dirX * parameters.RadiusStart,
                    parameters.CenterY + dirY * parameters.RadiusStart,
                    obj.Span.StartFrame);

                if (CanAnimate(obj.Span))
                    AddPosition(obj,
                        parameters.CenterX + dirX * parameters.RadiusEnd,
                        parameters.CenterY + dirY * parameters.RadiusEnd,
                        obj.Span.LastFrame, parameters.Ease);

                if (parameters.FaceOutward) AddRotation(obj, angle, obj.Span.StartFrame);
            }
        }

        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var travel = Travel(parameters.TravelFrames);
            var stagger = Stagger(parameters.StaggerFrames);
            var perObject = 2 + (parameters.FaceOutward ? 1 : 0);

            var keys = 0;

            var objects = 0;
            for (var i = 0; i < count; i++)
            {
                if (!CanSpawn(context, context.Span.StartFrame + i * stagger)) break;
                objects++;

                var span = ClampSpan(context, new FrameSpan(ClampFrame(context, context.Span.StartFrame + i * stagger), travel));
                keys += perObject + (CanAnimate(span) ? 2 : 1);
            }
            return new GeneratorCost(objects, keys);
        }

        private static int Count(int value) => value < 1 ? 1 : value;
        private static int Travel(int value) => value < 1 ? 1 : value;
        private static int Stagger(int value) => value < 0 ? 0 : value;

        public class Parameters : SpawnParameters
        {
            public int Count = 24;
            public float AngularStep = 17f;
            public float StartAngle;
            public float RadiusStart = 0.5f;
            public float RadiusEnd = 12f;
            public float CenterX;
            public float CenterY;
            public int TravelFrames = 90;
            public int StaggerFrames = 3;
            public EaseType Ease = EaseType.Linear;
            public bool FaceOutward = true;
        }
    }
}
