// Copy of UnityEngine.Color32 for no Unity uses

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BHSDK.Models.Interfaces;
using BHSDK.Utils;

namespace BHSDK.Models.Resources
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Pixel : IEquatable<Pixel>, IFormattable, ICopyable<Pixel>
    {
        [FieldOffset(0)] public int rgba;
        
        [FieldOffset(0)] public byte r;
        [FieldOffset(1)] public byte g;
        [FieldOffset(2)] public byte b;
        [FieldOffset(3)] public byte a;

        public Pixel(byte r, byte g, byte b, byte a)
        {
            this = default;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public Pixel(int rgba)
        {
            this = default;
            this.rgba = rgba;
        }

        public static Pixel Lerp(in Pixel a, in Pixel b, float t)
        {
            t = MathUtils.Clamp01(t);
            return new Pixel
            {
                r = (byte)(a.r + (b.r - a.r) * t),
                g = (byte)(a.g + (b.g - a.g) * t),
                b = (byte)(a.b + (b.b - a.b) * t),
                a = (byte)(a.a + (b.a - a.a) * t)
            };
        }
        public static Pixel LerpUnclamped(Pixel a, Pixel b, float t)
        {
            return new Pixel
            {
                r = (byte)(a.r + (b.r - a.r) * t),
                g = (byte)(a.g + (b.g - a.g) * t),
                b = (byte)(a.b + (b.b - a.b) * t),
                a = (byte)(a.a + (b.a - a.a) * t)
            };
        }

        public byte this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => r,
                    1 => g,
                    2 => b,
                    3 => a,
                    _ => throw new IndexOutOfRangeException($"Invalid Color32 index({index.ToString()})!")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: r = value; break;
                    case 1: g = value; break;
                    case 2: b = value; break;
                    case 3: a = value; break;
                    default: throw new IndexOutOfRangeException($"Invalid Color32 index({index.ToString()})!");
                }
            }
        }

        public readonly override int GetHashCode() => rgba.GetHashCode();

        public object Clone() => Copy();
        public Pixel Copy() => new(r, g, b, a);

        public readonly override bool Equals(object other)
        {
            return other is Pixel pixel && Equals(pixel);
        }
        public readonly bool Equals(Pixel other) => rgba == other.rgba;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override string ToString() => ToString(null, null);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(string format) => ToString(format, null);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
            return $"RGBA({r.ToString(format, formatProvider)}, {g.ToString(format, formatProvider)}, {b.ToString(format, formatProvider)}, {a.ToString(format, formatProvider)})";
        }
    }
}