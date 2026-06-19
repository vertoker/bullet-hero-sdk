using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Enum;

namespace BH.SDK.Rules
{
    public static class FrameRules
    {
        public const int MinFrame = 0;
        public const int MinFrameLength = 1;
        public const float MinTime = 0f;
        public const int MinFramerate = 1;
        
        public const float MinSpeed = -3f;
        public const float MaxSpeed = 3f;
        public const float DefaultSpeed = 1f;
        public const float SpeedStep = 0.1f;
        
        public const EaseType DefaultEase = EaseType.Linear;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidFramerate(int framerate) => framerate > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertFramerate(int framerate)
        {
            if (!IsValidFramerate(framerate))
                throw new Exception("Framerate must be greater than 0");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidTime(float time) => time >= 0f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertTime(float time)
        {
            if (!IsValidTime(time))
                throw new Exception("Time can't be negative");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidFrame(int frame) => frame >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertFrame(int frame)
        {
            if (!IsValidFrame(frame))
                throw new Exception("Frame can't be negative");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidSpeed(float speed) => speed is >= MinSpeed and <= MaxSpeed;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertSpeed(float speed)
        {
            if (!IsValidSpeed(speed))
                throw new Exception($"Speed must be in bounds ({MinSpeed}, {MaxSpeed})");
        }
    }
}