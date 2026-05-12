using System;
using BHSDK.Models.Interfaces;
using BHSDK.Utils;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Resources
{
    public class PixelTexture : ICopyable<PixelTexture>, IEquatable<PixelTexture>
    {
        public int Width;
        public int Height;
        public Pixel[] Pixels;

        public PixelTexture(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Pixel[width * height];
        }
        public PixelTexture(int width, int height, Pixel[] pixels)
        {
            Width = width;
            Height = height;
            Pixels = pixels;
        }

        public object Clone() => Copy();
        public PixelTexture Copy() => new(Width, Height, Pixels.CopyArray());

        public override bool Equals(object obj) => obj is PixelTexture value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Width, Height, Pixels.GetArrayHashCode());

        public bool Equals(PixelTexture other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Width.Equals(other.Width)
                         && Height.Equals(other.Height)
                         && Pixels.ArrayEquals(other.Pixels);
            return result;
        }
    }
}