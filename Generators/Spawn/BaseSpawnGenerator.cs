using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Generators.Spawn
{
    // What every geometry/bullet generator has in common is not the shape - it is the bookkeeping
    // around each spawned object: mint it, parent it, bound it to the context's frame range, give it
    // the template's texture/size/colour/collider, and only then place it. Concentrating that here
    // leaves a concrete generator as pure placement math, which is the part worth reading.
    //
    // Angles are DEGREES throughout this layer, matching AngleKey's own contract ("target rotation
    // in degrees"). The Unity project converts to radians at its own runtime boundary; a generator
    // must not do that conversion itself, or every rotation it writes comes out ~57x too small.

    /// <summary>
    /// Base for content generators that spawn objects from a shared template.
    /// </summary>
    public abstract class BaseSpawnGenerator<TParams> : BaseContentGenerator<TParams>
        where TParams : SpawnParameters, new()
    {
        /// <summary> Creates one templated object, already parented, framed and given its size and
        /// colour at startFrame. Placement (position/rotation) is the caller's job. </summary>
        protected static TextureObject Spawn(GeneratorContext context, SpawnParameters parameters,
            string name, int startFrame, int endFrame)
        {
            var obj = context.Create<TextureObject>();
            obj.ParentObjectId = context.Parent;
            obj.Name = name;
            obj.Layer = ClampLayer(context.Layer);
            obj.StartFrame = ClampFrame(context, startFrame);
            obj.EndFrame = ClampFrame(context, Math.Max(endFrame, startFrame));
            obj.TextureResourceId = parameters.Texture;
            obj.ColliderId = parameters.Collider;

            AddSize(obj, parameters.Size, obj.StartFrame);
            AddColor(obj, parameters.Color, obj.StartFrame);
            return obj;
        }

        /// <summary> Object count that fits in the context's own frame range - a generator staggering
        /// its output over time asks this before deciding how much it can actually place. </summary>
        protected static int FrameSpan(GeneratorContext context) => Math.Max(context.EndFrame - context.StartFrame, 0);

        protected static void AddPosition(TextureObject obj, float x, float y, int frame,
            EaseType ease = FrameRules.DefaultEase)
        {
            obj.Positions.Add(new PosKey(new Vector2Value(ClampPos(x), ClampPos(y)), frame, ease));
        }

        protected static void AddRotation(TextureObject obj, float degrees, int frame,
            EaseType ease = FrameRules.DefaultEase)
        {
            obj.Rotations.Add(new AngleKey(new FloatValue(degrees), frame, ease));
        }

        protected static void AddSize(TextureObject obj, IVector2 size, int frame,
            EaseType ease = FrameRules.DefaultEase)
        {
            obj.Sizes.Add(new ScaKey(size?.Copy() ?? new Vector2Value(1f, 1f), frame, ease));
        }

        protected static void AddSize(TextureObject obj, float width, float height, int frame,
            EaseType ease = FrameRules.DefaultEase)
        {
            obj.Sizes.Add(new ScaKey(new Vector2Value(ClampSize(width), ClampSize(height)), frame, ease));
        }

        /// <summary> Replaces the template size Spawn already wrote, for generators whose objects are
        /// individually sized (a line segment's length, a shrinking bullet). Replaces rather than
        /// appends because Frame must stay unique within a track - see RuleCollectionUnique. </summary>
        protected static void SetSize(TextureObject obj, float width, float height)
        {
            obj.Sizes.Clear();
            AddSize(obj, width, height, obj.StartFrame);
        }

        protected static void AddColor(TextureObject obj, IColor4 color, int frame,
            EaseType ease = FrameRules.DefaultEase)
        {
            obj.Colors.Add(new Color4Key(color?.Copy() ?? new Color4Value(1f, 1f, 1f, 1f), frame, ease));
        }

        protected static void AddColor(TextureObject obj, IColor4 color, float alpha, int frame,
            EaseType ease = FrameRules.DefaultEase)
        {
            var faded = color?.Copy() ?? new Color4Value(1f, 1f, 1f, 1f);
            if (faded is Color4Value literal) literal.A = Clamp01(alpha);
            obj.Colors.Add(new Color4Key(faded, frame, ease));
        }

        // Degrees in, unit vector out. System.Math is the only trigonometry available here - the
        // core SDK asmdef has noEngineReferences, so there is no Mathf.
        protected static void Direction(float degrees, out float x, out float y)
        {
            var radians = degrees * (Math.PI / 180.0);
            x = (float)Math.Cos(radians);
            y = (float)Math.Sin(radians);
        }

        protected static float Lerp(float from, float to, float t) => from + (to - from) * t;

        /// <summary> Even split of [0,1] across count steps: 0 for a single item (which then sits at
        /// `from` rather than jumping to `to`), i/(count-1) otherwise. </summary>
        protected static float Ratio(int index, int count) => count <= 1 ? 0f : index / (float)(count - 1);

        private static float ClampPos(float value) => Clamp(value, ValueRules.MinPos, ValueRules.MaxPos);
        private static float ClampSize(float value) => Clamp(value, ValueRules.MinSca, ValueRules.MaxSca);
        private static int ClampLayer(int value)
            => value < ValueRules.MinLayer ? ValueRules.MinLayer
                : value > ValueRules.MaxLayer ? ValueRules.MaxLayer : value;

        // Frames are clamped to the context's own window, not to the level's: a generator asked to
        // fill [100, 200] must not write outside it even when its own math overshoots, or an author
        // who scoped a run to one section finds objects in another.
        //
        // protected because an animated generator has to apply the SAME clamp inside its Estimate:
        // a bullet whose lifetime got truncated to a single frame carries one position key instead
        // of two, and an estimate that ignored that would drift from reality by exactly the amount
        // the clamp removed.
        protected static int ClampFrame(GeneratorContext context, int frame)
            => frame < context.StartFrame ? context.StartFrame
                : frame > context.EndFrame ? context.EndFrame : frame;

        /// <summary> True when an object's clamped lifetime still spans more than one frame, i.e.
        /// there is room for a second keyframe on any of its tracks. A collapsed lifetime must get
        /// ONE key per track - two on the same frame violates RuleCollectionUnique. </summary>
        protected static bool CanAnimate(int startFrame, int endFrame) => endFrame > startFrame;

        private static float Clamp(float value, float min, float max)
            => value < min ? min : value > max ? max : value;

        private static float Clamp01(float value) => Clamp(value, 0f, 1f);
    }
}
