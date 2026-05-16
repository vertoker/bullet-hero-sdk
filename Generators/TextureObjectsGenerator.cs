using BHSDK.Models;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Objects;
using BHSDK.Models.Values;
using BHSDK.Utils;

namespace BHSDK.Generators
{
    public class TextureObjectsGenerator : ILevelGenerator<TextureObjectsParameters>
    {
        public void Generate(Level level, TextureObjectsParameters parameters)
        {
            var widthMult = parameters.Width / (float)parameters.Texture.Width;
            var heightMult = parameters.Height / (float)parameters.Texture.Height;

            var parentObj = new Object();
            parentObj.SetObjectId(level.Settings);
            parentObj.SetBounds(parameters.StartFrame, parameters.EndFrame);
            parentObj.Layer = parameters.Layer;
            level.Game.Objects.Add(parentObj);
            
            var indexer = new DimensionalIndexer2(parameters.Width, parameters.Height);
            using var enumerator = indexer.GetEnumerator2();
            while (enumerator.MoveNext())
            {
                var pixel = parameters.Texture.Pixels[indexer.GetIndex(enumerator.Current)];
                var mappedIndexX = (int)(enumerator.Current.IndexWidth * widthMult);
                var mappedIndexY = (int)(enumerator.Current.IndexHeight * heightMult);

                var textureObj = new TextureObject();
                textureObj.SetObjectId(level.Settings);
                textureObj.SetParent(parentObj);
                textureObj.SetBounds(parameters.StartFrame, parameters.EndFrame);
                parentObj.Layer = parameters.Layer;

                var posVec = new Vector2Value(mappedIndexX, -mappedIndexY);
                var pos = new Pos(posVec, parameters.PixelAnchor.Copy(), parameters.StartFrame);
                var sca = new Sca(parameters.PixelSca.Copy(), parameters.StartFrame);
                var clr = new Clr(pixel.ToColorValue(), parameters.StartFrame);
                
                textureObj.Positions.Add(pos);
                textureObj.Scales.Add(sca);
                textureObj.Colors.Add(clr);
            }
        }
    }
}