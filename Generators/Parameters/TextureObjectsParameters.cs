using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using BHSDK.Rules;

namespace BHSDK.Generators
{
    public class TextureObjectsParameters : ObjectsParameters
    {
        public readonly PixelTexture Texture;
        public readonly int Width;
        public readonly int Height;
        public readonly Alignment PixelAnchor;
        public readonly Vector2Value PixelSca;

        public TextureObjectsParameters(PixelTexture texture,
            int startFrame, int endFrame, int layer = ValueRules.ObjectLayerDefault) : base(startFrame, endFrame, layer)
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
            PixelAnchor = Alignment.MiddleCenter;
            PixelSca = new Vector2Value(1f, 1f);
        }
        public TextureObjectsParameters(PixelTexture texture, int width, int height, Alignment pixelAnchor, Vector2Value pixelSca,
            int startFrame, int endFrame, int layer = ValueRules.ObjectLayerDefault) : base(startFrame, endFrame, layer)
        {
            Texture = texture;
            Width = width;
            Height = height;
            PixelAnchor = pixelAnchor;
            PixelSca = pixelSca;
        }
    }
}