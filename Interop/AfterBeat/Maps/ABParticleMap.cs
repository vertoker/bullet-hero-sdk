using System;
using BH.SDK.Interop.AfterBeat.Models;

namespace BH.SDK.Interop.AfterBeat
{
    // THE PARAMETERS ARE NOT IN "csp". BeatmapObject declares ParticleSpawnRatePerSecondValueIndex
    // ... ParticleStartSpeedValueIndex directly under the csp field, which reads as if indices 4-11
    // continued the custom-shape array. They do not: every accessor goes through
    // GetParticleSettingValue, which reads events[0].keyframes[0].GetVal(index, default) - the
    // FIRST POSITION KEYFRAME's own value array. csp stays the five-float shape half, and no
    // ot = 7 object in the measured corpus carries one at all.
    //
    // Each index carries its own default and its own clamp, and neither is decoration: the default
    // for world space is 1 and for the arc 360, so reading a short array as zeroes - which is what
    // VgdKeyframe.GetValue answers, correctly, for every other caller - turns a full circle into no
    // circle. ReadValue below is the source game's GetVal(index, default), and it is the only way
    // this file reads a value.

    /// <summary> Afterbeat's particle emitter parameters, read off the object's first position
    /// keyframe. </summary>
    public static class ABParticleMap
    {
        #region Indices, defaults and bounds

        public const int SpawnRatePerSecondIndex = 4;
        public const int SpawnRatePerUnitIndex = 5;
        public const int WorldSpaceIndex = 6;
        public const int DespawnOnEndIndex = 7;
        public const int EmitterShapeIndex = 8;
        public const int EmitterArcIndex = 9;
        public const int EmitterRadiusThicknessIndex = 10;
        public const int StartSpeedIndex = 11;

        public const float SpawnRatePerSecondDefault = 0f;
        public const float SpawnRatePerUnitDefault = 0f;
        public const float WorldSpaceDefault = 1f;
        public const float DespawnOnEndDefault = 0f;
        public const float EmitterShapeDefault = 0f;
        public const float EmitterArcDefault = 360f;
        public const float EmitterRadiusThicknessDefault = 1f;
        public const float StartSpeedDefault = 1f;

        /// <summary> What the source game reads a stored float as a bool with - not a comparison
        /// against zero, so 0.4 is false and 0.5 is true. </summary>
        public const float TruthThreshold = 0.5f;

        public const float MaxEmitterArc = 360f;

        // THE HIDDEN CHANNELS. An emitter's four tracks do two jobs at once: values 0/1 keep their
        // ordinary meaning and animate the EMITTER, while values 2/3 describe ONE PARTICLE over its
        // own life. Both directions of the converter read the same indices, which is why they live
        // here rather than in either one of them.

        /// <summary> Where a position keyframe keeps a particle's own travel over its life. </summary>
        public const int ParticleVelocityXIndex = 2;

        /// <summary> And the other axis of it. </summary>
        public const int ParticleVelocityYIndex = 3;

        /// <summary> Where a scale keyframe keeps a particle's own width over its life. </summary>
        public const int ParticleScaleXIndex = 2;

        /// <summary> And its height. </summary>
        public const int ParticleScaleYIndex = 3;

        /// <summary> Where a rotation keyframe keeps a particle's own angle over its life. </summary>
        public const int ParticleAngleIndex = 2;

        /// <summary> An unauthored size channel means a particle that never changes size. </summary>
        public const float ParticleScaleDefault = 1f;

        /// <summary> And an unauthored angle channel means one that never turns. </summary>
        public const float ParticleAngleDefault = 0f;

        /// <summary> What an emitter carrying no scale keyframe spawns inside. </summary>
        public const float DefaultEmitterExtent = 1f;

        /// <summary> A circle emitter's radius is half the extent its scale describes. </summary>
        public const float EmitterRadiusOfExtent = 0.5f;

        /// <summary> Tracks 0-3 - the four an object has. Anything past them is not a timeline. </summary>
        public const int TimelineTrackCount = 4;

        /// <summary> The shortest particle life the source game will resolve, whatever the
        /// keyframes say. </summary>
        public const float MinTimelineLength = 0.01f;

        /// <summary> What an object carrying no tracks at all resolves to - one second, not the
        /// floor. The source game answers this before it ever looks at a keyframe. </summary>
        public const float NoTracksTimelineLength = 1f;

        #endregion

        /// <summary> Whether this source object is a particle emitter. </summary>
        public static bool IsEmitter(VgdObject source)
            => source != null && (ABObjectType)source.ObjectType == ABObjectType.Particles;

        /// <summary> One emitter's parameters, or null when the object is not an emitter. Every
        /// field an emitter did not author comes back as the source game's own default. </summary>
        public static ABParticleSettings? TryRead(VgdObject source)
        {
            if (!IsEmitter(source)) return null;

            var keyframe = GetSettingsKeyframe(source);

            return new ABParticleSettings(
                spawnRatePerSecond: Math.Max(0f,
                    ReadValue(keyframe, SpawnRatePerSecondIndex, SpawnRatePerSecondDefault)),
                spawnRatePerUnit: Math.Max(0f,
                    ReadValue(keyframe, SpawnRatePerUnitIndex, SpawnRatePerUnitDefault)),
                worldSpace: ReadValue(keyframe, WorldSpaceIndex, WorldSpaceDefault) >= TruthThreshold,
                despawnOnEnd: ReadValue(keyframe, DespawnOnEndIndex, DespawnOnEndDefault) >= TruthThreshold,
                emitterShape: ReadEmitterShape(keyframe),
                emitterArc: Math.Clamp(
                    ReadValue(keyframe, EmitterArcIndex, EmitterArcDefault), 0f, MaxEmitterArc),
                emitterRadiusThickness: Math.Clamp(
                    ReadValue(keyframe, EmitterRadiusThicknessIndex, EmitterRadiusThicknessDefault),
                    0f, 1f),
                startSpeed: Math.Max(0f, ReadValue(keyframe, StartSpeedIndex, StartSpeedDefault)),
                timelineLength: ResolveTimelineLength(source));
        }

        // A PARTICLE'S LIFE IS NOT THE OBJECT'S LIFETIME, and it is not ABTimeMap's answer either.
        // GetLastKeyframeTime skips any track holding one keyframe or none, because the source game
        // skips it when deciding how long the OBJECT lives (GetLongestSequence). Particle life is a
        // different question resolved by different code (ResolveParticleTimelineLength), and that
        // one counts every keyframe it can see - so an emitter whose only animated track is its
        // colour still gets its particles' life from it.

        /// <summary> How long one particle lives, in seconds: the largest keyframe time across the
        /// object's four tracks, floored so it can never be zero. </summary>
        public static float ResolveTimelineLength(VgdObject source)
        {
            if (source?.Tracks == null) return NoTracksTimelineLength;

            var last = 0f;
            var count = Math.Min(TimelineTrackCount, source.Tracks.Count);

            for (var i = 0; i < count; i++)
            {
                var keyframes = source.Tracks[i]?.Keyframes;
                if (keyframes == null) continue;

                foreach (var keyframe in keyframes)
                    if (keyframe != null && keyframe.Time > last) last = keyframe.Time;
            }

            return Math.Max(MinTimelineLength, last);
        }

        /// <summary> The keyframe every parameter is read off, or null when the object carries
        /// none - in which case every parameter is its own default. </summary>
        private static VgdKeyframe GetSettingsKeyframe(VgdObject source)
        {
            var keyframes = source?.Move?.Keyframes;
            return keyframes == null || keyframes.Count == 0 ? null : keyframes[0];
        }

        /// <summary> The source game's GetVal(index, default): a value the file did not write is
        /// the DEFAULT, never zero. </summary>
        private static float ReadValue(VgdKeyframe keyframe, int index, float fallback)
        {
            var values = keyframe?.Values;
            return values != null && index >= 0 && index < values.Count ? values[index] : fallback;
        }

        /// <summary> Rectangle for everything that is not exactly 1, matching the source game's own
        /// rounded test - an out-of-range number is a box rather than an error. </summary>
        private static ABParticleEmitterShapeType ReadEmitterShape(VgdKeyframe keyframe)
        {
            var raw = ReadValue(keyframe, EmitterShapeIndex, EmitterShapeDefault);
            var rounded = (int)Math.Round(raw, MidpointRounding.AwayFromZero);

            return rounded == (int)ABParticleEmitterShapeType.Circle
                ? ABParticleEmitterShapeType.Circle
                : ABParticleEmitterShapeType.Rectangle;
        }
    }

    /// <summary> One Afterbeat emitter's own parameters, already defaulted and clamped. </summary>
    public readonly struct ABParticleSettings
    {
        /// <summary> Particles per second. </summary>
        public float SpawnRatePerSecond { get; }

        /// <summary> Particles per unit travelled - emission by distance, which this format has no
        /// counterpart for at all. </summary>
        public float SpawnRatePerUnit { get; }

        /// <summary> Whether particles are left behind as the emitter travels, rather than dragged
        /// along with it. </summary>
        public bool WorldSpace { get; }

        /// <summary> Whether the particles alive when emission ends are killed with it. </summary>
        public bool DespawnOnEnd { get; }

        /// <summary> Which volume particles spawn inside. </summary>
        public ABParticleEmitterShapeType EmitterShape { get; }

        /// <summary> Portion of the emitter circle used, in DEGREES. Circle only. </summary>
        public float EmitterArc { get; }

        /// <summary> How far inward the emitter ring is filled, 0 to 1. Circle only. </summary>
        public float EmitterRadiusThickness { get; }

        /// <summary> Speed along the emitter shape's own normal - radially outward for a circle,
        /// along +Z for a box, which in a 2D scene is invisible. </summary>
        public float StartSpeed { get; }

        /// <summary> How long one particle lives, in seconds. </summary>
        public float TimelineLength { get; }

        public ABParticleSettings(float spawnRatePerSecond, float spawnRatePerUnit, bool worldSpace,
            bool despawnOnEnd, ABParticleEmitterShapeType emitterShape, float emitterArc,
            float emitterRadiusThickness, float startSpeed, float timelineLength)
        {
            SpawnRatePerSecond = spawnRatePerSecond;
            SpawnRatePerUnit = spawnRatePerUnit;
            WorldSpace = worldSpace;
            DespawnOnEnd = despawnOnEnd;
            EmitterShape = emitterShape;
            EmitterArc = emitterArc;
            EmitterRadiusThickness = emitterRadiusThickness;
            StartSpeed = startSpeed;
            TimelineLength = timelineLength;
        }
    }
}
