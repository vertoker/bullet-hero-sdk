using BHSDK.Models.Interfaces;
using BHSDK.Utils;

namespace BHSDK.Models.Resources
{
    public class PixelTexture : ICopyable<PixelTexture>
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
    }
}