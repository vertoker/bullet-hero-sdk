using BH.SDK.Models.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Generators
{
    public class TextureObjectsParameters : ObjectsParameters
    {
        public readonly PixelTexture Texture;
        public readonly int Width;
        public readonly int Height;
        public readonly Alignment PixelPivot;
        public readonly Vector2Value PixelSca;

        public TextureObjectsParameters(PixelTexture texture,
            int startFrame, int endFrame, int layer = ValueRules.DefaultLayer) : base(startFrame, endFrame, layer)
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
            PixelPivot = Alignment.CenterMiddle;
            PixelSca = new Vector2Value(1f, 1f);
        }
        public TextureObjectsParameters(PixelTexture texture, int width, int height, Alignment pixelPivot, Vector2Value pixelSca,
            int startFrame, int endFrame, int layer = ValueRules.DefaultLayer) : base(startFrame, endFrame, layer)
        {
            Texture = texture;
            Width = width;
            Height = height;
            PixelPivot = pixelPivot;
            PixelSca = pixelSca;
        }
    }
}