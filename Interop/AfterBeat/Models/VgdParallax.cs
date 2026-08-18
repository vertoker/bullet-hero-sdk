using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // A parallax object is a different KIND of thing from a gameplay object, not a variant of one:
    // it has no keyframes at all, only a static transform plus an optional loop it interpolates
    // towards forever. This project has no parallax subsystem, so the importer turns each of these
    // into an ordinary collider-less object with the loop baked into keyframes.
    //
    // Watch the key collisions here - "s" at the top level of a parallax object is the SHAPE
    // object, while "t.s" and "an.s" are scale VECTORS.

    /// <summary> The .vgd parallax_settings block - five background layers plus their depth of field. </summary>
    public class VgdParallaxSettings : AfterBeatNode
    {
        public const int LayerCount = 5;

        [JsonProperty(AfterBeatNames.ParallaxLayers)]
        public List<VgdParallaxLayer> Layers { get; set; } = new();

        /// <summary> Zero-indexed main layer; -1 for none. </summary>
        [JsonProperty(AfterBeatNames.ParallaxMainLayer)]
        public int MainLayer { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxDofActive)]
        public bool DepthOfFieldActive { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxDofValue)]
        public int DepthOfFieldValue { get; set; }
    }

    /// <summary> One parallax layer. </summary>
    public class VgdParallaxLayer : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.ParallaxLayerDepth)]
        public int Depth { get; set; }

        /// <summary> Index into the theme's parallax palette; overrides each object's own. </summary>
        [JsonProperty(AfterBeatNames.ParallaxLayerColor)]
        public int Color { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxLayerObjects)]
        public List<VgdParallaxObject> Objects { get; set; } = new();
    }

    /// <summary> One background object. </summary>
    public class VgdParallaxObject : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(AfterBeatNames.ParallaxObjectShape)]
        public VgdParallaxShape Shape { get; set; } = new();

        /// <summary> Index into the theme's parallax palette; the layer's own may override it. </summary>
        [JsonProperty(AfterBeatNames.ParallaxObjectColor)]
        public int Color { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxObjectTransform)]
        public VgdParallaxTransform Transform { get; set; } = new();

        [JsonProperty(AfterBeatNames.ParallaxObjectAnimation)]
        public VgdParallaxAnimation Animation { get; set; } = new();
    }

    /// <summary> The shape/option pair, nested here rather than flat as it is on a gameplay object. </summary>
    public class VgdParallaxShape : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.ObjectShape)]
        public int Shape { get; set; }

        [JsonProperty(AfterBeatNames.ObjectShapeOption)]
        public int ShapeOption { get; set; }
    }

    /// <summary> A parallax object's static transform. Rotation is in DEGREES here, unlike an
    /// object keyframe's, and is absolute rather than relative to anything. </summary>
    public class VgdParallaxTransform : AfterBeatNode
    {
        [JsonProperty(AfterBeatNames.ParallaxTransformPosition)]
        public VgdVector2 Position { get; set; } = new();

        [JsonProperty(AfterBeatNames.ParallaxTransformScale)]
        public VgdVector2 Scale { get; set; } = new();

        [JsonProperty(AfterBeatNames.ParallaxTransformRotation)]
        public float Rotation { get; set; }
    }

    /// <summary> The endless loop a parallax object interpolates towards. Each of the three
    /// switches is independent, so a layer can breathe in scale while standing still. </summary>
    public class VgdParallaxAnimation : AfterBeatNode
    {
        /// <summary> Loop period in seconds. </summary>
        [JsonProperty(AfterBeatNames.ParallaxAnimationLength)]
        public float Length { get; set; }

        /// <summary> Phase - where the loop starts, in seconds from the start of the level. </summary>
        [JsonProperty(AfterBeatNames.ParallaxAnimationDelay)]
        public float Delay { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxAnimationLoopPosition)]
        public bool LoopPosition { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxAnimationLoopScale)]
        public bool LoopScale { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxAnimationLoopRotation)]
        public bool LoopRotation { get; set; }

        [JsonProperty(AfterBeatNames.ParallaxTransformPosition)]
        public VgdVector2 Position { get; set; } = new();

        [JsonProperty(AfterBeatNames.ParallaxTransformScale)]
        public VgdVector2 Scale { get; set; } = new();

        [JsonProperty(AfterBeatNames.ParallaxTransformRotation)]
        public float Rotation { get; set; }

        /// <summary> True when anything at all loops - a zero-length loop animates nothing however
        /// its switches are set. </summary>
        [JsonIgnore]
        public bool IsActive => Length > 0f && (LoopPosition || LoopScale || LoopRotation);
    }
}
