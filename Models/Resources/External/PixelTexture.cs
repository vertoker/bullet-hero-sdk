namespace BHSDK.Models.Resources
{
    public class PixelTexture
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
    }
}