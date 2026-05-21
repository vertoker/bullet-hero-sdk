using BH.SDK.Models.Resources;
using UnityEngine;

namespace BH.SDK
{
    public static class ResourceExtensions
    {
#if BHSDK_UNITY
        public static Color32 ToColor32(this Pixel pixel) => new(pixel.r, pixel.g, pixel.b, pixel.a);
        public static Pixel ToPixel(this Color32 color) => new(color.r, color.g, color.b, color.a);

        public static PixelTexture ToPixelTexture(this Texture2D texture)
        {
            var pixels = texture.GetPixelData<Pixel>(0);
            var pixelTexture = new PixelTexture(texture.width, texture.height, pixels.ToArray());
            pixels.Dispose();
            return pixelTexture;
        }
        public static Texture2D ToTexture2D(this PixelTexture pixelTexture)
        {
            var texture = new Texture2D(pixelTexture.Width, pixelTexture.Height);
            texture.SetPixelData(pixelTexture.Pixels, 0);
            texture.Apply();
            return texture;
        }
#endif
    }
}