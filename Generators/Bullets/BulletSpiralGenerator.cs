using BH.SDK.Generators.Spawn;
using BH.SDK.Models.Enum;
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
            .Order(nameof(Parameters.Count), nameof(Parameters.AngularStep), nameof(Parameters.StartAngle),
                nameof(Parameters.RadiusStart), nameof(Parameters.RadiusEnd),
                nameof(Parameters.CenterX), nameof(Parameters.CenterY),
                nameof(Parameters.TravelFrames), nameof(Parameters.StaggerFrames),
                nameof(Parameters.Ease), nameof(Parameters.FaceOutward))
            .Order(SpawnParameters.FieldOrder)
            .Range(nameof(Parameters.Count), 1, 512)
            .Range(nameof(Parameters.AngularStep), -360f, 360f)
            .Range(nameof(Parameters.RadiusStart), 0f, ValueRules.MaxPos)
            .Range(nameof(Parameters.RadiusEnd), 0f, ValueRules.MaxPos)
            .Range(nameof(Parameters.TravelFrames), 1, FrameRules.MaxFrameLength)
            .Range(nameof(Parameters.StaggerFrames), 0, FrameRules.MaxFrameLength)
            .Unit(nameof(Parameters.AngularStep), "deg")
            .Unit(nameof(Parameters.StartAngle), "deg")
            .Unit(nameof(Parameters.TravelFrames), "frames")
            .Unit(nameof(Parameters.StaggerFrames), "frames")
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
                var spawnFrame = context.StartFrame + i * stagger;

                var obj = Spawn(context, parameters, $"spiral_{i}", spawnFrame, spawnFrame + travel);
                AddPosition(obj,
                    parameters.CenterX + dirX * parameters.RadiusStart,
                    parameters.CenterY + dirY * parameters.RadiusStart,
                    obj.StartFrame);

                if (CanAnimate(obj.StartFrame, obj.EndFrame))
                    AddPosition(obj,
                        parameters.CenterX + dirX * parameters.RadiusEnd,
                        parameters.CenterY + dirY * parameters.RadiusEnd,
                        obj.EndFrame, parameters.Ease);

                if (parameters.FaceOutward) AddRotation(obj, angle, obj.StartFrame);
            }
        }

        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        {
            var count = Count(parameters.Count);
            var travel = Travel(parameters.TravelFrames);
            var stagger = Stagger(parameters.StaggerFrames);
            var perObject = 2 + (parameters.FaceOutward ? 1 : 0);

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
